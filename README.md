AsposeOCRExample
================

A console application that demonstrates one way of the Aspose.OCR for .NET usage.


How to recognize text from GroupDocs image using Aspose.OCR for .NET
====================================================================

![Aspose OCR For Cloud](http://www.aspose.com/Images/aspose_ocr_for_cloud_140px.png "Aspose OCR For Cloud")

This article show you a brief example how you can make an OCR from your GroupDocs image on the client side.

To perform this task we have to be able to make three things:

  1. Get access to the GroupDocs service.
  2. Get an image from the GroupDocs service.
  3. Use Aspose OCR API to recognize text from the downloaded image.

Let's get started.

To recognize text from an image you already have simply use [OCREnginge](http://www.aspose.com/docs/display/ocrnet/OcrEngine+Class).

``` c#
	using Aspose.OCR;
	...
	String imageFilePath = @"image.tiff";
	OcrEngine ocr = new OcrEngine();
	ocr.Image = ImageStream.FromFile(imageFilePath);
	ocr.Languages.AddLanguage(Language.Load("english"));
	using(ocr.Resource = new FileStream(resourceFileName, FileMode.Open))
	{
		if (ocr.Process())
		{
			//Use ocr.Text - parsed result lives there.
		}
	}
```

How to obtain the representation of the GroupDocs service to download an image you can see in on of the [API samples](http://groupdocs.com/cloud/sdk-examples) as provided below.

``` c#
	using Groupdocs.Sdk;

	...

	//Set variables
	String clientId = "<USERS CLIENT ID>";
	String privateKey = "<USERS PRIVATE KEY>";

	//Set base path to the dev server
	String basePath = "https://api.groupdocs.com/v2.0";

	//Create service for Groupdocs account
	GroupdocsService service = new GroupdocsService(basePath, clientId, privateKey);
```

You can get your client id and private key at the profile page (see [here](http://groupdocs.com/docs/display/documentation/How+to+Get+Your+GroupDocs+API+Keys)).

And now to download an image use the obtained GroupdocsService, provide correct file id that points to the image you want to parse and get it using Storage API.

``` c#
	String fileId = "<FILE ID>";

	//Get all files and folders from account.
	Groupdocs.Api.Contract.ListEntitiesResult files =
	  service.GetFileSystemEntities("", 0, -1, null, true, null, null);

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
		//Process it from LocalPath + name
	}
```

So now you only have to use the downloaded file to process it.

``` c#
	String imageFilePath = LocalPath + name;

	OcrEngine ocr = new OcrEngine();
	ocr.Image = ImageStream.FromFile(imageFilePath);
	ocr.Languages.AddLanguage(Language.Load("english"));
	string resourceFileName = @"<RESOURCES FILE PATH>";
	using(ocr.Resource = new FileStream(resourceFileName, FileMode.Open))
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
```

To use it you have to provide (reference) [Aspose.OCR for .NET Component](http://www.aspose.com/.net/ocr-component.aspx) and use an appropriate [resource file](http://www.aspose.com/community/files/51/.net-components/aspose.ocr-for-.net/category1404.aspx).
And of course include the GroupDocs SDK (one can use [nuget package](https://www.nuget.org/packages/groupdocs-dotnet/)).

