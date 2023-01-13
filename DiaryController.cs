namespace MongoDBLabb
{
    internal class DiaryController
    {
        IStringIO io;
        IEntryDAO entryDAO;

        public DiaryController(IStringIO io, IEntryDAO entryDAO)
        {
            this.io = io;
            this.entryDAO = entryDAO;
        }

        public void Start()
        {
            int val = Menu();
            if (val<1)
            {
                Menu(); //måste kolla, kanske loop? 
            }

            switch(val)
            {
                case 1:
                    entryDAO.GetAllEntries().ForEach(entry => { io.PrintString(entry); });
                    break;
                case 2:
                    //kolla hur man ska ta in input för vad som ska skapas, eventuellt ändra i interfacet så det stödjer det man vill göra 
                    entryDAO.CreateEntry();
                    break;
                case 3:
                    ReadEntriesMenu();
                    string input = io.GetString(); // Läs in med felhantering 
                    entryDAO.GetEntriesByFilter(input);
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    io.Exit();
                    break;
            }


            
            //loop? Meny för vad man vill göra.  Köra privata metoder här? 


        }

        private int Menu()
        {
            io.PrintString("Min Dagbok");
            io.PrintString("1. Visa alla inlägg" +
                         "\n2. Skriv ett inlägg" +
                         "\n3. Sök efter inlägg" +
                         "\n4. Redigera inlägg" +
                         "\n5. Radera inlägg" +
                         "\n6. Avsluta");         //Göra till en lista som skrivs ut?

            io.PrintString("Skriv siffran som motsvarar ditt val");

            int answer;
            bool success = int.TryParse(io.GetString(), out answer);

            if (success)
            {
                if(answer<1 & answer>6) //kör efter count i listan istället för hårdkodat?
                {
                    io.PrintString("Felaktig input, vänligen välj bland tillgängliga rubriker.");
                    Menu();
                }
                return answer;
            }
            io.PrintString("Felaktig input, skriv endast siffror. Välj bland rubrikerna.");
            Menu();
            return 0; // kolla hur jag ska göra så att menyn körs efter fel

        }

        private int ReadEntriesMenu()   //Gör till lista och skriv ut
        {
            io.PrintString("Sök efter inlägg");    
            io.PrintString("1. Sök på datum" +
                         "\n2. Sök efter text i inlägg" +
                         "\n3. Sök på titel" +
                         "\n4. Avsluta");

            return 0;
        }
    }
}
