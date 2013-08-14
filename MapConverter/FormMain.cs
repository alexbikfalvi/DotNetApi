﻿using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using Catfood.Shapefile;

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
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
				{
					// Open the ZIP archive.
					using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
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

								// Write the bounding box of this shape file.
								this.textBox.AppendText(string.Format("Bounds: {0},{1} -> {2},{3} {4}",
									shapefile.BoundingBox.Left,
									shapefile.BoundingBox.Top,
									shapefile.BoundingBox.Right,
									shapefile.BoundingBox.Bottom,
									Environment.NewLine));

								// Enumerate all shapes.
								foreach (Shape shape in shapefile)
								{
									//ShapePoint
									this.textBox.AppendText(string.Format("{0} {1} {2}", shape.Type, shape.RecordNumber, Environment.NewLine));
								}
								this.textBox.AppendText(Environment.NewLine);
							}
						}
						finally
						{
							// Delete the temporary folder.
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

			this.textBox.AppendText("Done.");
		}
	}
}
