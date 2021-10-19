using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services
{
    public class SearchIndex
    {
        private const string URL = "url";
        private const string TITLE = "title";
        private const string TEXT = "text";

        // Ensures index backward compatibility
        private const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

        private readonly IndexWriter indexWriter;
        private readonly FSDirectory fSDirectory;

        public SearchIndex()
        {
            // Construct a machine-independent path for the index
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            string indexPath = Path.Combine(basePath, "index");

            System.IO.Directory.Delete(indexPath, true);

            this.fSDirectory = FSDirectory.Open(indexPath);

            // Create an analyzer to process the text
            StandardAnalyzer standardAnalyzer = new StandardAnalyzer(AppLuceneVersion);

            // Create an index writer
            IndexWriterConfig indexWriterConfig = new IndexWriterConfig(AppLuceneVersion, standardAnalyzer);

            this.indexWriter = new IndexWriter(fSDirectory, indexWriterConfig);
        }

        public void Write(string url, string title, string text)
        {
            Document document = new Document
            {
                new StringField(URL, url, Field.Store.YES),
                new StringField(TITLE, title, Field.Store.YES),
                new TextField(TEXT, text, Field.Store.YES),
            };

            indexWriter.AddDocument(document);

            indexWriter.Flush(false, false);
        }

        public IEnumerable<Result> Search(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                yield break;
            }

            IEnumerable<string> terms = searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            MultiPhraseQuery multiPhraseQuery = new MultiPhraseQuery
            {
                terms.Select(term => new Term(TEXT, term)).ToArray()
            };

            using (DirectoryReader directoryReader = indexWriter.GetReader(true))
            {
                IndexSearcher indexSearcher = new IndexSearcher(directoryReader);

                ScoreDoc[] scoreDocs = indexSearcher.Search(multiPhraseQuery, 20).ScoreDocs;

                foreach (var hit in scoreDocs)
                {
                    Document document = indexSearcher.Doc(hit.Doc);

                    yield return new Result { Url = document.Get(URL), Title = document.Get(TITLE), Score = hit.Score };
                }
            }
        }
    }
}
