## Simple Distributed Web Crawler

## What it is?

Functional demonstration of 
1. Distributed archetecture
2. Event driven system
4. Message-Oriented Middleware Using ZeroMQ
5. A basic web crawler
6. Scalibility
7. Microservices architecture 
8. The producer-consumer pattern
9. Extract, transform, and load (ETL) process
10. Stream processing
11. Asyoncronous processing
![Process diagram](DWC.png?raw=true)
## Workers:
1. Download Worker: 
                    Activated by UrlFilteredEvent
                    Downloads html for the url specified by UrlFilteredEvent.
                    Publishes HtmlDownloadedForTextExtractionEvent and HtmlDownloadedForUrlExtractionEvent
                    
2. Url Extraction Worker: 
                   Activated by HtmlDownloadedForUrlExtractionEvent
                   Extracts links/Urls from the html
                   Publishes UrlExtractedEvent

3. Url Examiner Worker:
                   Activated by UrlExtractedEvent
                   Examines whether the extracted url is valid http resource and the server is rechable
                   Publishes UrlExaminedEvent

4. Url Filter Worker:
                   Activated by UrlExaminedEvent
                   Check whether this url is already downloaded or not using Bloomfilter
                   Publishes UrlFilteredEvent

5. Text Extraction Worker:
                   Activated by HtmlDownloadedForTextExtractionEvent
                   Extracts the text from html by removing all the html tags.
                   Publishes TextExtractedEvent

6. Search Index Worker:
                   Activated by TextExtractedEvent
                   Indexes text in Lucene for searching
                   Exposes http api for searching
                   
## Notes:
Download, Url Extraction, Url Examiner, Text Extraction workers are scalable, that means you may run as many instances of this processes you want. Url filter hosts a in memory bloom filter which is not distributed. To do so you need to incorporate REDIS based bloom filter or something so. Search index worker also hosts an in-memory Lucene search engine exposed by a thin web service and therefore not scalable, you should use Solr or Elastic search to do so. I used ZeroMQ for distributed messaging which in my opinion needs to be replaced by RabbitMQ for high throughput and performance. You should be able to search through http://localhost:5000/Search?query=kasim. https://www.wikipedia.org/ which is the seed.

