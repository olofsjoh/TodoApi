using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;

namespace TodoApp
{
    class XmlFacade : Facade
    {
        public XmlFacade(string baseAddress) :base(baseAddress)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
        }

        protected override List<Todo> Deserialize(string content)
        {
            var serializer = new XmlSerializer(typeof(List<Todo>));

            using (TextReader reader = new StringReader(content))
            {
                return (List<Todo>) serializer.Deserialize(reader);
            }
        }
    }
}
