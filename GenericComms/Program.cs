using GenericComms;
PythonCommunications<string> test = await PythonCommunications<string>.GetConnection("http://localhost").PerformGetAsync("get");
if (test.IsFaulted){
    Console.WriteLine(test.FaultMessage);
}

