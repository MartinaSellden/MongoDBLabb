
using MongoDBLabb;


IStringIO io;
IEntryDAO entryDAO;

io = new TextIO();
entryDAO = new MongoDAO(connectionString, "Entries");

DiaryController controller = new DiaryController(io, entryDAO);



controller.Start();