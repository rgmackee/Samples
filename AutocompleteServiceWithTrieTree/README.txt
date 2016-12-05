**************************************************************************************************************************************
This project shows the implementation of a Trie data structure. This is most popularly used for suffix and prefix searches, such as 
auto-completion, which is what this project is about.
If you run the project it will render a default html page used to showcase the feature. This is a Web API project that holds the REST 
service that powers the auto-completion feature on the textbox via ajax calls. The auto-completion is set to kick in from the second
letter typed on. Only letters and at most one hyphen are accepted as valid input (Example: Ann-Marie), but no spaces are allowed
(Example: Jean Paul).
There are also unit tests that cover pretty much every scenario when calling the service. 