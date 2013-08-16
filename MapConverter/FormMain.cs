using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using Catfood.Shapefile;
using DotNetApi;
using DotNetApi.IO;
using DotNetApi.Xml;
using MapApi;

namespace MapConverter
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void OnProcess(object sender, EventArgs e)
		{
			// Clear the text box.
			this.textBox.Clear();
			// Load the file.
			if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				this.OnOpenFile(this.openFileDialog.FileName);
			}
		}

		private void OnOpenFile(string fileName)
		{
			// Show message.
			this.textBox.AppendText(string.Format("Opening file \'{0}\'...{1}", fileName, Environment.NewLine));

			try
			{
				// Open a stream to the ZIP file.
				using (FileStream fileInStream = new FileStream(fileName, FileMode.Open))
				{
					// Open the ZIP archive.
					using (ZipArchive zipArchive = new ZipArchive(fileInStream, ZipArchiveMode.Read))
					{
						// The shape file name.
						string shapeFileName = null;

						this.textBox.AppendText(string.Format("Extracting shape ZIP archive...{0}", Environment.NewLine));
						foreach (ZipArchiveEntry entry in zipArchive.Entries)
						{
							// If this is the shape file, save the name.
							if (Path.GetExtension(entry.Name) == ".shp")
							{
								shapeFileName = entry.Name;
							}
							this.textBox.AppendText(string.Format("- {0}: {1} bytes {2} bytes compressed{3}", entry.Name, entry.Length, entry.CompressedLength, Environment.NewLine));
						}

						// If there are no entries, throw an exception.
						if (null == shapeFileName) throw new FileNotFoundException("The ZIP archive does not contain a shape file.");

						// Create the name of a temporary folder.
						string tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

						this.textBox.AppendText(string.Format("Shape file name is: \'{0}\'{1}", shapeFileName, Environment.NewLine));

						// Create the temporary folder.
						Directory.CreateDirectory(tempFolder);

						this.textBox.AppendText(string.Format("Creating temporary folder \'{0}\'...{1}", tempFolder, Environment.NewLine));

						try
						{
							// Extract the shapefile contents.
							zipArchive.ExtractToDirectory(tempFolder);

							// Open the shapefile.
							using (Shapefile shapefile = new Shapefile(Path.Combine(tempFolder, shapeFileName)))
							{
								this.textBox.AppendText(Environment.NewLine);

								// Write the basic information.
								this.textBox.AppendText(string.Format("Type: {0}, Shapes: {1:n0}{2}", shapefile.Type, shapefile.Count, Environment.NewLine));

								this.textBox.AppendText(Environment.NewLine);

								// Create a map object.
								Map map = new Map(new MapRectangle(
									shapefile.BoundingBox.Left,
									shapefile.BoundingBox.Top,
									shapefile.BoundingBox.Right,
									shapefile.BoundingBox.Bottom));

								// Write the bounding box of this shape file.
								this.textBox.AppendText(string.Format("Bounds: {0},{1} -> {2},{3}{4}",
									shapefile.BoundingBox.Left,
									shapefile.BoundingBox.Top,
									shapefile.BoundingBox.Right,
									shapefile.BoundingBox.Bottom,
									Environment.NewLine));

								// Enumerate all shapes.
								foreach (Shape shape in shapefile)
								{
									// Shape basic information.
									//this.textBox.AppendText(string.Format("{0} {1} {2} ", shape.RecordNumber, shape.Type, shape.GetMetadata("name")));

									// Create a new shape.
									MapShape mapShape;
									switch (shape.Type)
									{
										case ShapeType.Point:
											ShapePoint shapePoint = shape as ShapePoint;
											mapShape = new MapShapePoint(new MapPoint(shapePoint.Point.X, shapePoint.Point.Y));
											break;
										case ShapeType.Polygon:
											ShapePolygon shapePolygon = shape as ShapePolygon;

											//this.textBox.AppendText(string.Format(": {0}", shapePolygon.Parts.Count));

											MapShapePolygon mapShapePolygon = new MapShapePolygon(new MapRectangle(
												shapePolygon.BoundingBox.Left,
												shapePolygon.BoundingBox.Top,
												shapePolygon.BoundingBox.Right,
												shapePolygon.BoundingBox.Bottom));
											foreach(PointD[] part in shapePolygon.Parts)
											{
												MapPart mapPart = new MapPart();
												foreach (PointD point in part)
												{
													mapPart.Points.Add(point.X, point.Y);
												}
												mapShapePolygon.Parts.Add(mapPart);
											}
											mapShape = mapShapePolygon;
											break;
										default:
											throw new NotSupportedException(string.Format("Shape type {0} is not supported.", shape.Type));
									}
									// Add the shape metadata.
									foreach (string name in shape.GetMetadataNames())
									{
										mapShape.Metadata[name] = shape.GetMetadata(name);
									}
									// Add the shape to the map.
									map.Shapes.Add(mapShape);
									//this.textBox.AppendText(Environment.NewLine);
								}

								this.textBox.AppendText(Environment.NewLine);

								// Serialize the map object.
								XmlSerializer serializer = new XmlSerializer(typeof(Map));
								// Create a string writer.
								using (StringWriter writer = new StringWriter())
								{
									// Serialize the map data.
									serializer.Serialize(writer, map);
									// Display the XML.
									this.textBox.AppendText(writer.ToString());

									this.textBox.AppendText(Environment.NewLine);
									this.textBox.AppendText(Environment.NewLine);

									// Display a dialog to save the file.
									if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK)
									{
										// Create a file stream.
										using (FileStream fileOutStream = File.Create(this.saveFileDialog.FileName))
										{
											using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(writer.ToString())))
											{
												// Compress the stream.
												using (GZipStream zipStream = new GZipStream(fileOutStream, CompressionLevel.Optimal))
												{
													this.textBox.AppendText("Uncompressed data is {0} bytes.{1}".FormatWith(memoryStream.Length, Environment.NewLine));
													memoryStream.CopyTo(zipStream);
													this.textBox.AppendText("Compressed data is {0} bytes.{1}".FormatWith(fileOutStream.Length, Environment.NewLine));
												}
											}
										}
									}
								}
								//this.textBox.AppendText(map.ToXml().ToString());
								this.textBox.AppendText(Environment.NewLine);
							}
						}
						finally
						{
							// Delete the temporary folder.
							this.textBox.AppendText(Environment.NewLine);
							Directory.Delete(tempFolder, true);
							this.textBox.AppendText(string.Format("Temporary folder \'{0}\' deleted.{1}", tempFolder, Environment.NewLine));
						}
					}
				}
			}
			catch (Exception exception)
			{
				this.textBox.AppendText(string.Format("An exception occurred. {0}", exception.Message));
			}
			this.textBox.AppendText(Environment.NewLine);
			this.textBox.AppendText("Done.");
		}
	}
}
