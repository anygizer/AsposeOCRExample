using System;
using System.IO;

using Groupdocs.Sdk;

using Aspose.OCR;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{

			//Set variables
			String clientId = "<USERS CLIENT ID>";
			String privateKey = "<USERS PRIVATE KEY>";

			//Set base path to the dev server
			String basePath = "https://api.groupdocs.com/v2.0";

			//Create service for Groupdocs account
			GroupdocsService service = new GroupdocsService(basePath, clientId, privateKey);

			String fileId = "<FILE ID>";

			//Get all files and folders from account.
			Groupdocs.Api.Contract.ListEntitiesResult files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);

			//Create empty variable for file name
			String name = null;

			//Check if files are not null
			if (files.Files != null)
			{
				//Obtaining file name for entered file Id
				for (int i = 0; i < files.Files.Length; i++)
				{
					if (files.Files[i].Guid == fileId)
					{
						name = files.Files[i].Name;
					}
				}
			}

			//Definition of folder where to download file
			String LocalPath = AppDomain.CurrentDomain.BaseDirectory;

			//Make a request to Storage Api for dowloading file
			bool file = service.DownloadFile(fileId, LocalPath + name);

			//If file downloaded successful
			if (file)
			{
				String imageFilePath = LocalPath + name;
				Console.WriteLine(imageFilePath);

				OcrEngine ocr = new OcrEngine();
				ocr.Image = ImageStream.FromFile(imageFilePath);
				ocr.Languages.AddLanguage(Language.Load("english"));
				string resourceFileName = @"<RESOURCES FILE PATH>";
				using (ocr.Resource = new FileStream(resourceFileName, FileMode.Open))
				{
					try
					{
						if (ocr.Process())
						{
							Console.WriteLine("Parsed result:" + Environment.NewLine + ocr.Text);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Exception: " + ex.Message);
					}
				}

				//Clean after yourself
				File.Delete(imageFilePath);

				//Wait a minute...
				Console.Read();
			}
		}
	}
}
