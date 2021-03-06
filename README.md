Enosql - embedded nosql database, based on Chrome V8 engine
==============================================================
This project will try to mimic the MongoDB C# driver as much as possible.  
So that its painless to switch to that when your database grows.  

## Usage
Enosql will take care of creating the database if it doesn't exist
and creating the collection if it doesn't exist. Just like with MongoDB.
It will throw an error if the database folder does not have read/write permissions.

        public class Tasks
        {
            public string _id { get; set; }
            public string task { get; set; }
            public DateTime duedate { get; set; }
            public string category { get; set; }
            public string status { get; set; }
        }

        private void addTask()
        {
            var db = new enosql.EnosqlDatabase(@"c:\temp\Todo.jdb");
            var TaskCollection = db.GetCollection<Tasks>();
            TaskCollection.Insert(new Tasks()
            {
                task = "Pick up flowers",
                duedate = DateTime.Now.AddDays(2),
                category = "anniversary",
                status = "unfinished"
            });
        }

## Installation
If you don't feel like building from source just use these binaries.  
**Download:** [Enosql v 0.8.0](https://s3.amazonaws.com/dv.github2/Enosql-v0.8.0.7z)

## Performance
        insert 5000 records, 100k each - Serialize Object - write scheduled every 100 msec
        no warm up
        ---------------------------------------------
        00:00:01.3900000 sec
        00:00:01.3600000 sec
        00:00:01.3670000 sec
        
        with warm up
        ---------------------------------------------
        00:00:01.5670000 sec
        00:00:01.2840000 sec
        00:00:01.6270000 sec

## Build & Dependency Requirements   
  Current build solution uses VS2012  
  Dependencies
  - V8.NET - https://v8dotnet.codeplex.com/
  - Json.NET - http://james.newtonking.com/pages/json-net.aspx


## Credits
This project would not exist without the great V8.NET wrapper.  
https://v8dotnet.codeplex.com/

## License
The MIT License (MIT)

Copyright (c) 2013 Donny Velazquez

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
