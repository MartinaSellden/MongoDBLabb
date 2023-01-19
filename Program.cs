
using MongoDBLabb;


IUI io;
IDAO entryDAO;

io = new TextIO();
entryDAO = new MongoDAO(ConnectionString.connectionStr, "Diary");

DiaryController controller = new DiaryController(io, entryDAO);

await controller.StartAsync();