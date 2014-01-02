using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using DotNetApi;
using DotNetApi.Globalization;
using DotNetApi.Windows.Forms;

namespace LocaleConverter
{
	public partial class FormMain : ThreadSafeForm
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			// The list of locales.
			HashSet<string> locales = new HashSet<string>();
			foreach (string locale in this.textBoxLocales.Text.Split(new char[] { ' ', ',', ';', '/' }, StringSplitOptions.RemoveEmptyEntries))
			{
				locales.Add(locale);
			}

			// If the list of locales is empty, do nothing.
			if (locales.Count == 0)
			{
				this.textBoxOutput.AppendText("Matching all locales.");
			}

			// Disable the controls.
			this.buttonLoad.Enabled = false;
			this.textBoxLocales.Enabled = false;

			// Set the dialog properties.
			this.openFileDialog.Filter = "CLDR ZIP files (*.zip)|*.zip";
			this.openFileDialog.Title = "Open CLDR Zip File";

			// Load the CLDR file.
			if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				// Get the file name.
				string fileName = this.openFileDialog.FileName;

				ThreadPool.QueueUserWorkItem((object state) =>
					{
						try
						{
							// Open the CLDR file.
							using (FileStream fileZip = new FileStream(fileName, FileMode.Open))
							{
								// Open the ZIP archive.
								using (ZipArchive zip = new ZipArchive(fileZip, ZipArchiveMode.Read))
								{
									// Create a locale collection.
									LocaleCollection localeCollection = new LocaleCollection();

									// Parse all entries.
									foreach (ZipArchiveEntry zipEntry in zip.Entries)
									{
										// If the file name does not match the common path, skip too next file.
										if (!Regex.IsMatch(zipEntry.FullName, @"^common/main/.+?\.xml$")) continue;
										if (!Regex.IsMatch(zipEntry.Name, @"^.+?\.xml$")) continue;

										// If there is a list of locales.
										if (locales.Count > 0)
										{
											// Get the locale name.
											string localeName = zipEntry.Name.Substring(0, zipEntry.Name.Length - 4);

											// If the locale name does not exist, continue.
											if (!locales.Contains(localeName)) continue;
										}

										// Get a temporary file name.
										string tempFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

										this.Invoke(() =>
											{
												// Show a message.
												this.textBoxOutput.AppendText("Processing file \'{0}\'... ".FormatWith(zipEntry.Name));
											});

										try
										{
											// Extract the file entry to the specified file.
											zipEntry.ExtractToFile(tempFileName, true);

											// Parse the file.
											Locale locale = this.Parse(tempFileName);

											// Add the locale to the collection.
											localeCollection.Add(locale);

											this.Invoke(() =>
												{
													this.textBoxOutput.AppendText("OK {0}".FormatWith(Environment.NewLine));
												});
										}
										catch (Exception ex)
										{
											this.Invoke(() =>
												{
													this.textBoxOutput.AppendText("FAIL {0}{1}".FormatWith(ex.Message, Environment.NewLine));
												});
										}
										finally
										{
											try
											{
												// Delete the file.
												File.Delete(tempFileName);
											}
											catch { }
										}
									}

									this.Invoke(() =>
										{
											// Save the locale collection to a resource file.
											if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK)
											{
												try
												{
													using (FileStream file = new FileStream(this.saveFileDialog.FileName, FileMode.Create))
													{
														using (LocaleWriter writer = new LocaleWriter(file))
														{
															writer.WriteLocaleCollection(localeCollection);
														}
													}
												}
												catch (Exception exception)
												{
													this.textBoxOutput.AppendText("Error: {0}".FormatWith(exception.Message));
												}

												// Create a resource writer for the specified file.
												//using (ResXResourceWriter resx = new ResXResourceWriter(this.saveFileDialog.FileName))
												//{
												//	//  Serialize the locale collection.
												//	resx.AddResource("LocaleCollection", localeCollection);
												//}
											}
										});
								}
							}
						}
						catch (Exception exception)
						{
							this.Invoke(() =>
								{
									this.textBoxOutput.AppendText("Error: {0}".FormatWith(exception.Message));
								});
						}
						finally
						{
							this.Invoke(() =>
								{
									// Enable the controls.
									this.buttonLoad.Enabled = true;
									this.textBoxLocales.Enabled = true;
								});
						}
					});
			}
		}

		/// <summary>
		/// Parses the specified XML file.
		/// </summary>
		/// <param name="fileName">The file.</param>
		/// <returns>The locale.</returns>
		private Locale Parse(string fileName)
		{
			// Open the XML file.
			using (FileStream fileXml = new FileStream(fileName, FileMode.Open))
			{
				// Read the XML document.
				XDocument xml = XDocument.Load(fileXml);

				// Check the root element.
				if (xml.Root.Name != "ldml") throw new XmlException("The XML file is not a LDML document.");

				// Parse the locale identity.
				Locale locale = new Locale(this.ParseIdentity(xml.Root.Element("identity")));

				// Parse the locale display names.
				this.ParseDisplayNames(xml.Root.Element("localeDisplayNames"), ref locale);

				// Return the locale.
				return locale;
			}
		}

		/// <summary>
		/// Parses the locale identity from the specified identity element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The locale culture identity.</returns>
		private CultureId ParseIdentity(XElement element)
		{
			XElement el;
			string language = element.Element("language").Attribute("type").Value;
			string script = ((el = element.Element("script")) != null) ? el.Attribute("type").Value : null;
			string territory = ((el = element.Element("territory")) != null) ? el.Attribute("type").Value : null;
			return new CultureId(language, script, territory);
		}

		/// <summary>
		/// Parses the locale display names from the specified element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="locale">The locale.</param>
		private void ParseDisplayNames(XElement element, ref Locale locale)
		{
			// Parse the locale languages.
			this.ParseLanguages(element.Element("languages"), ref locale);

			// Parse the locale scripts.
			this.ParseScripts(element.Element("scripts"), ref locale);

			// Parse the locale territories.
			this.ParseTerritories(element.Element("territories"), ref locale);
		}

		/// <summary>
		/// Parses the locale languages from the specified element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="locale">The locale.</param>
		private void ParseLanguages(XElement element, ref Locale locale)
		{
			foreach (XElement el in element.Elements("language"))
			{
				// Skip the draft languages.
				if (el.Attribute("draft") != null) continue;
				// Skip the alternate languages.
				if (el.Attribute("alt") != null) continue;

				// Add the language to the locale.
				locale.Languages.Add(el.Attribute("type").Value, el.Value);
			}
		}

		/// <summary>
		/// Parses the locale scripts from the specified element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="locale">The locale.</param>
		private void ParseScripts(XElement element, ref Locale locale)
		{
			foreach (XElement el in element.Elements("script"))
			{
				// Skip the draft languages.
				if (el.Attribute("draft") != null) continue;
				// Skip the alternate languages.
				if (el.Attribute("alt") != null) continue;

				// Add the script to the locale.
				locale.Scripts.Add(el.Attribute("type").Value, el.Value);
			}
		}

		/// <summary>
		/// Parses the locale territories from the specified element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="locale">The locale.</param>
		private void ParseTerritories(XElement element, ref Locale locale)
		{
			foreach (XElement el in element.Elements("territory"))
			{
				// Skip the draft territories.
				if (el.Attribute("draft") != null) continue;
				// Skip the alternate territories.
				if (el.Attribute("alt") != null) continue;

				// Add the territory to the locale.
				locale.Territories.Add(el.Attribute("type").Value, el.Value);
			}
		}

		/// <summary>
		/// Reads and checks a locale XML file.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnRead(object sender, EventArgs e)
		{
			// Set the dialog properties.
			this.openFileDialog.Filter = "XML files (*.xml)|*.xml";
			this.openFileDialog.Title = "Open Locales XML File";

			// Open the file.
			if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				this.textBoxOutput.AppendText("Reading the locales XML file...");

				try
				{
					// Open the file stream.
					using (FileStream file = new FileStream(this.openFileDialog.FileName, FileMode.Open))
					{
						// Open the locale reader.
						using (LocaleReader reader = new LocaleReader(file))
						{
							LocaleCollection locales = reader.ReadLocaleCollection();
						}
					}

					this.textBoxOutput.AppendText("Success.");
				}
				catch (Exception exception)
				{
					this.textBoxOutput.AppendText("Error: {0}".FormatWith(exception.Message));
				}
			}
		}
	}
}
