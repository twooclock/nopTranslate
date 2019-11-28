nopTranslate
===========
nopTranslate helps translating [nopCommerce](https://github.com/nopSolutions/nopCommerce) e-commerce shopping cart solution. 

To translate nopCommerce one has to translate language_pack.xml file with 6471 strings (as of version 4.3).


nopTranslate splits this file into smaller txt files which you can feed to [translate.google.com](translate.google.com) later those files are joined back to form a newly translated language_pack.xml.


Whole process of translating a new language took me less than half an hour where most of the time was spent on "cleaning" google translate results.

nopTranslate is written as .net core 3.0 console application.
