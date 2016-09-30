# PhotoFiler
A simple MVC 5 online photo album written in C#.  

Demo [here](http://www.adriandemavivas.com/PhotoFiler/). 

Application Settings
--------------------
* **RoothPath** - Root path of the the photos.  The path would be scanned recursivedly looking for JPEG and PNG files. 
* **HashLength** - Length of the hash string.  The hash string is used as the name of the preview file of the photos. 
* **CreatePreview** - Flag to indicate if previews are created.  Previews are created in the App_Data folder. 
* **EnableLogging** - Flag to indicate if logging is enabled.  Uses System.Diagnostics.TraceSource to create an XmlWriterTraceListener configured in the configuration file. 
