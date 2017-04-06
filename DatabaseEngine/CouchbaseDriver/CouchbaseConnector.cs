using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Configuration.Client;
using CouchN.Test;

namespace DatabaseEngine.CouchbaseDriver
{
    public class CouchbaseConnector
    {
        public void TestCouch()
        {

            var cluster = new Cluster(new ClientConfiguration {ApiPort = 7092,DirectPort = 11211,Servers = new List<Uri> {new Uri("http://192.168.74.132")} });
            using (var bucket = cluster.OpenBucket("test"))
            {
                var document = new Document<dynamic>
                {
                    Id = "Hello",
                    Content = new
                    {
                        name = "Couchbase"
                    }
                };

                var upsert = bucket.Upsert(document);
                if (!upsert.Success) return;
                var get = bucket.GetDocument<dynamic>(document.Id);
                document = get.Document;
                var msg = string.Format("{0} {1}!", document.Id, document.Content.name);
                Console.WriteLine(msg);
            }
        }
    }

    public class TestDoc
    {
        public string Text { get; set; }
    }
}
