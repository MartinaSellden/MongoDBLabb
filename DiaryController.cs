

using MongoDB.Bson;
using SharpCompress.Common;
using System.Collections.Specialized;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
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

        public async Task StartAsync()     // Fixa felhantering inkl. kolla all input   Eventuellt kolla att bara vissa årtal, timmar etc tillåts (behövs nog inte eftersom den sätter datetime.now hela tiden)
        {
            try
            {
                do
                {
                    int choice = Menu();

                    switch (choice)
                    {
                        case 1:

                            GetAllEntries();

                            break;
                        case 2:

                            await CreateEntryAsync();

                            break;
                        case 3:

                            int selectedNumber = ReadEntriesMenu();
                            if (selectedNumber == 1)
                            {
                                ReadByDate();
                            }
                            if (selectedNumber == 2)
                            {
                                ReadById();
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

                            await UpdateEntryAsync();
                            break;
                        case 5:

                            await DeleteEntryAsync();
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

        private void ReadById()
        {
            try
            {
                bool tryAgain = true;

                while (tryAgain)
                {
                    io.PrintString("Skriv in id-nummer för inlägget du vill hitta.");

                    ObjectId id;
                    bool success = ObjectId.TryParse(io.GetString(), out id);

                    if (success)
                    {
                        EntryODM retrievedEntry = entryDAO.ReadEntryById(id);

                        if (retrievedEntry==null)
                        {
                            io.PrintString($"\nInlägg med id-nummer \"{id}\" kunde inte hittas.");
                            tryAgain= false;
                            io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                        }
                        else
                        {
                            io.PrintString("\nId-nummer: " + retrievedEntry.Id + 
                                              "\nDatum: " + retrievedEntry.Date + 
                                              "\nTitel: " + retrievedEntry.Title + 
                                              "\nInlägg: " + retrievedEntry.Content + "\n............................................");

                            tryAgain= false;
                            io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
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

        private async Task DeleteEntryAsync()
        {
            bool tryAgain = true;

            while (tryAgain)
            {
                io.PrintString("Skriv in id-nummer för inlägget du vill radera.");

                ObjectId id;
                bool success = ObjectId.TryParse(io.GetString(), out id);

                if (success)
                {
                    var entryToDelete = entryDAO.ReadEntryById(id);
                    if (entryToDelete == null)
                    {
                        io.PrintString("\nInget inlägg med valt datum kunde hittas.");
                        tryAgain= false;
                        io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                        Console.ReadKey();
                    }
                    else
                    {
                        await entryDAO.DeleteEntryAsync(id);
                        io.PrintString("\nInlägg raderat");
                        tryAgain = false;
                        io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    io.PrintString("Felaktigt format.");
                }
            }
        }

        private async Task UpdateEntryAsync()
        {
            try
            {
                bool tryAgain = true;

                while (tryAgain)
                {
                    io.PrintString("Skriv in id-nummer för inlägget du vill redigera.");

                    ObjectId id;
                    bool success = ObjectId.TryParse(io.GetString(), out id);

                    if (success)
                    {
                        EntryODM retrievedEntry = entryDAO.ReadEntryById(id);

                        if (retrievedEntry==null)
                        {
                            io.PrintString($"\nInlägg med id-nummer \"{id}\" kunde inte hittas.");
                            tryAgain= false;
                            io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                        }
                        else
                        {
                            io.PrintString("Skriv in ny text för inlägget.");
                            string newContent = io.GetString();

                            await entryDAO.UpdateEntryAsync(id, newContent);
                            tryAgain= false;
                            io.PrintString("\nInlägg redigerat.");
                            io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        io.PrintString("Felaktigt format.");
                        io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task CreateEntryAsync()
        {
            try
            {
                io.PrintString("Skriv titel för inlägget");
                string title = io.GetString();

                io.PrintString("Skriv ditt inlägg");
                string content = io.GetString();

                EntryODM entry = new EntryODM(title, content);
                await entryDAO.CreateEntryAsync(entry);

                io.PrintString("\nInlägg skapat.");
                io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                Console.ReadKey();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void GetAllEntries()
        {
            try
            {
                List<EntryODM> allEntries = entryDAO.ReadAllEntries();

                io.PrintString("............................................");

                allEntries.ForEach(entry => io.PrintString("Id-nummer: " +
                                                             entry.Id + "\nDatum: " +
                                                             entry.Date + "\nTitel: " +
                                                             entry.Title + "\nInlägg: " +
                                                             entry.Content + "\n............................................")); ;

                io.PrintString(" ");
                io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                Console.ReadKey();

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
                        var entriesByDate = entryDAO.ReadEntriesByFilter("shortDate", date);
                        if (entriesByDate.Count ==0)
                        {
                            io.PrintString("Det finns inget inlägg med valt datum.");
                            tryAgain = false;
                            io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                        }
                        else
                        {
                            entriesByDate.ForEach(entry => io.PrintString("\n..........................\nId-nummer: " +
                                                                             entry.Id + "\nDatum: " +
                                                                             entry.Date + "\nTitel: " +
                                                                             entry.Title + "\nInlägg: \n" +
                                                                             entry.Content));
                            tryAgain = false;

                            io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        io.PrintString("\nFelaktigt format.");
                        io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                        Console.ReadKey();

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

                var entryByTitle = entryDAO.ReadEntriesByFilter("title", titleForEntryToRead);

                if (entryByTitle.Count==0)
                {
                    io.PrintString($"\nInlägg med titel {titleForEntryToRead} kunde inte hittas");
                    io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                    Console.ReadKey();
                }
                else
                {
                    entryByTitle.ForEach(entry => io.PrintString("\n..........................\nId-nummer: " +
                                                                   entry.Id + "\nDatum: " +
                                                                   entry.Date + "\nTitel: " +
                                                                   entry.Title + "\nInlägg: \n" +
                                                                   entry.Content));

                    io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                    Console.ReadKey();
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

                io.PrintString(Art.title);

                io.PrintString("... Min Dagbok ......");

                foreach (var choice in menuChoices)
                    io.PrintString(choice);

                io.PrintString("\nSkriv siffran som motsvarar ditt val.");

                int answer;
                bool success = int.TryParse(io.GetString(), out answer);

                if (success)
                {
                    if (answer<1 || answer>menuChoices.Count)
                    {
                        io.PrintString("Felaktig input, vänligen välj bland tillgängliga rubriker.");
                        io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                        Console.ReadKey();
                        Menu();
                    }
                    return answer;
                }
                io.PrintString("Felaktig input, skriv endast siffror. Välj bland rubrikerna.");
                io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                Console.ReadKey();
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
                        "2. Sök på id-nummer",
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
                            io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                            Menu();
                        }

                        return answer;
                    }
                    io.PrintString("Felaktig input, skriv endast siffror. Välj bland rubrikerna.");
                    io.PrintString("\nTryck på valfri tangent för att fortsätta.");
                    Console.ReadKey();
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
