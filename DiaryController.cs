

using MongoDB.Bson;
using System.Collections.Specialized;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

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

        public void Start()     // Fixa felhantering inkl. kolla all input
        {
            try
            {
                do
                {
                    int choice = Menu();

                    switch (choice)
                    {
                        case 1:

                            ReadAllEntries();

                            break;
                        case 2:

                            CreateEntry();

                            break;
                        case 3:

                            int selectedNumber = ReadEntriesMenu();
                            if (selectedNumber == 1)
                            {
                                ReadByDate();
                            }
                            if (selectedNumber == 2)
                            {
                                // gör en ReadByEntryText();
                                //kolla om det går att söka på del av text och matcha
                            }
                            if (selectedNumber == 3)
                            {
                                ReadByTitle();
                            }
                            if (selectedNumber == 5)
                            {
                                io.Exit();
                            }
                            break;
                        case 4:
                           
                            UpdateEntry();
                            break;
                        case 5:

                            DeleteEntry();               
                            break;
                        case 6:

                            io.Exit();
                            break;
                    }

                } while (true);
            }
            catch (Exception e)
            {
                io.PrintString("Ett oväntat fel inträffade: " + e.ToString());
            }
        }

        private void DeleteEntry()
        {
            bool tryAgain  = true;
            Regex longDate = new Regex(@"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}");

            while (tryAgain)
            {
                io.PrintString("Skriv in datum enligt format YYYY-MM-DD hh:mm:ss");
                string dateForEntryToDelete = io.GetString();

                if (longDate.IsMatch(dateForEntryToDelete))
                {
                    var entryToDelete = entryDAO.GetEntriesByFilter("date", dateForEntryToDelete);
                    if (entryToDelete.Count == 0)
                    {
                        io.PrintString("\nInget inlägg med valt datum kunde hittas.");
                        tryAgain= false;
                    }
                    else
                    {
                        entryDAO.DeleteEntryAsync(dateForEntryToDelete);
                        io.PrintString("\nInlägg raderat");
                        tryAgain = false;
                    }
                }
                else
                {
                    io.PrintString("Felaktigt format.");
                }
            }
        }

        private void UpdateEntry()
        {
            try
            {
                bool tryAgain = true;
                Regex longDate = new Regex(@"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}");

                while (tryAgain)
                {
                    io.PrintString("Skriv in datum för inlägget du vill redigera. Använd format YYYY-MM-DD hh:mm:ss.");

                    string dateOfEntryToUpdate = io.GetString();
                    if (longDate.IsMatch(dateOfEntryToUpdate))
                    {
                        var retrievedEntries = entryDAO.GetEntriesByFilter("date", dateOfEntryToUpdate);
                        if (retrievedEntries.Count ==0)
                        {
                            io.PrintString("Det finns inget inlägg med valt datum.");
                            tryAgain= false;
                        }
                        else
                        {
                            io.PrintString("Skriv in ny text för inlägget.");
                            string newContent = io.GetString();

                            entryDAO.UpdateEntryAsync(dateOfEntryToUpdate, newContent);

                            io.PrintString("\nInlägg redigerat.");
                            tryAgain = false;
                        }
                    }
                    else
                    {
                        io.PrintString("Felaktigt format.");

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CreateEntry()
        {
            try
            {
                io.PrintString("Skriv titel för inlägget");
                string title = io.GetString();

                io.PrintString("Skriv ditt inlägg");
                string content = io.GetString();

                Entry entry = new Entry(title, content);
                entryDAO.CreateEntryAsync(entry);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ReadAllEntries()
        {
            try
            {
                List<Entry> allEntries = entryDAO.GetAllEntries();
                allEntries.ForEach(entry => io.PrintString("Id-nummer: " +
                                                             entry.Id + "\nDatum: " +
                                                             entry.Date + "\nTitel: " +
                                                             entry.Title + "\nInlägg: \n" +
                                                             entry.Content + "\n............................"));

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ReadByDate()
        {
            Regex shortDate = new Regex(@"\d{4}-\d{2}-\d{2}");
            bool tryAgain = true;

            try
            {
                while (tryAgain)
                {
                    io.PrintString("Visa alla inlägg för ett visst datum. Skriv in datum enligt format YYYY-MM-DD");

                    string date = io.GetString();
                    if (shortDate.IsMatch(date))
                    {
                        var entriesByDate = entryDAO.GetEntriesByFilter("shortDate", date);
                        if (entriesByDate.Count ==0)
                        {
                            io.PrintString("Det finns inget inlägg med valt datum.");
                            tryAgain = false;
                        }
                        else
                        {
                            entriesByDate.ForEach(entry => io.PrintString("\n..........................\nId-nummer: " +
                                                                             entry.Id + "\nDatum: " +
                                                                             entry.Date + "\nTitel: " +
                                                                             entry.Title + "\nInlägg: \n" +
                                                                             entry.Content));
                            tryAgain = false;
                        }
                    }
                    else
                    {
                        io.PrintString("\nFelaktigt format.");

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ReadByTitle()
        {
            try
            {
                io.PrintString("Skriv in titel för inlägget du vill visa.");
                string titleForEntryToRead = io.GetString();

                var entryByTitle = entryDAO.GetEntriesByFilter("title", titleForEntryToRead);

                if (entryByTitle.Count==0)
                {
                    io.PrintString($"\nInlägg med titel {titleForEntryToRead} kunde inte hittas");
                }
                else
                {
                    entryByTitle.ForEach(entry => io.PrintString("\n..........................\nId-nummer: " +
                                                                   entry.Id + "\nDatum: " +
                                                                   entry.Date + "\nTitel: " +
                                                                   entry.Title + "\nInlägg: \n" +
                                                                   entry.Content));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int Menu()
        {
            try
            {
                List<string> menuChoices = new List<string>
               {"\n1. Visa alla inlägg",
                "2. Skriv ett inlägg",
                "3. Sök efter inlägg",
                "4. Redigera inlägg",
                "5. Radera inlägg",
                "6. Avsluta" };

                io.PrintString("---------------------------------------------\n");
                io.PrintString("Min Dagbok\n");
                io.PrintString("....................");

                foreach (var choice in menuChoices)
                    io.PrintString($"{choice}");

                io.PrintString("\nSkriv siffran som motsvarar ditt val.");

                int answer;
                bool success = int.TryParse(io.GetString(), out answer);

                if (success)
                {
                    if (answer<1 || answer>menuChoices.Count)
                    {
                        io.PrintString("Felaktig input, vänligen välj bland tillgängliga rubriker.");
                        Menu();
                    }
                    return answer;
                }
                io.PrintString("Felaktig input, skriv endast siffror. Välj bland rubrikerna.");
                Menu();
                return 0;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private int ReadEntriesMenu()
        {
            try
            {
                bool showMenu = true;
                do
                {
                    List<string> readEntriesChoices = new List<string>
                    {   "1. Sök på datum",
                        "2. Sök efter text i inlägg",
                        "3. Sök på titel",
                        "4. Tillbaka till huvudmenyn",
                        "5. Avsluta"};

                    io.PrintString("Sök efter inlägg");

                    foreach (var choice in readEntriesChoices)
                        io.PrintString($"{choice}");

                    io.PrintString("\nSkriv siffran som motsvarar ditt val.");

                    int answer;
                    bool success = int.TryParse(io.GetString(), out answer);

                    if (success)
                    {
                        if (answer<1 || answer>readEntriesChoices.Count)
                        {
                            io.PrintString("Felaktig input, vänligen välj bland tillgängliga rubriker.");
                            Menu();
                        }

                        return answer;
                    }
                    io.PrintString("Felaktig input, skriv endast siffror. Välj bland rubrikerna.");
                    Menu();
                    return 0;

                }
                while (showMenu);
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
