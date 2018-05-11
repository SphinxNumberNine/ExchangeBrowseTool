# ExchangeBrowseTool
Tool made during internship at Commvault Systems to browse archived mail data from local Solr server, and allow options to perform operations on the server/data.

NOTE: Any user downloading this project from github will not be able to test the project. The project was developed using a proprietary SOLR schema that I was not given permission to upload. However, the user can test all of the UI elements, view the code used to access the webservers, and get a feel of what the project does given the proper schema and servers.

The code is broken up into folders based on the different aspects of what the application does:

The Menus folder contains any class relating to a menu in the application. Examples include Details menus for individual mail items, options menu for the whole application, etc.

The Queries folder contains any class relating to firing queries to the SOLR servers or to the webservers. The class SolrQuery contains the most generic queries, with other classes, like AtomicUpdate and CustomQuery existing for advanced commands and changes to the server.

The Reports folder contains any class relating to showing the user a report about the health of the SOLR server. The folder contains the actual report form, as well as the sub-forms that it uses.

The Handlers class contains utility classes that help organize certain aspects of the code. For example, the XMLParser class parses data from the SOLR server and grabs exactly the data that the application needs, storing it in a structure. Some UI handlers also exist in this folder, for organization's sake.

--------------------------------------------------------------------------------------------------------------


