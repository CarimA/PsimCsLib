using PsimCsLib.Models;
using PsimCsLib.PubSub;
using System.Text;

namespace PsimCsLib.Modules;
internal class ProcessRawData : ISubscriber<RawData>
{
    private readonly PsimClient _client;

    public ProcessRawData(PsimClient client)
    {
        _client = client;
    }

    public async Task HandleEvent(RawData e)
    {
        var message = Encoding.UTF8.GetString(e.Buffer);
        await ProcessMessage(message);
    }

    private async Task ProcessMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        if (message.IndexOf('\n') > -1)
        {
            var split = message.Split('\n');
            var room = "lobby";
            var start = 0;

            if (split[0][0] == '>')
            {
                start = 1;
                room = split[0][1..];
                if (string.IsNullOrWhiteSpace(room))
                    room = "lobby";
            }

            for (var index = start; index < split.Length; index++)
            {
                var item = split[index].Split('|')[1];
                if (item == "init")
                {
                    for (var subIndex = index; subIndex < split.Length; subIndex++)
                    {
                        await ProcessMessage(room, split[subIndex], true);
                        index = subIndex;
                    }
                }
                else
                {
                    await ProcessMessage(room, split[index]);
                }
            }
        }
        else
        {
            await ProcessMessage("lobby", message);
        }
    }

    private async Task ProcessMessage(string room, string message, bool isIntro = false)
    {
        var data = message.Split('|').Skip(1).ToList();

        if (data.Count == 0)
            return;

        var roomInstance = _client.Rooms[room];
        var command = data.First();
        var args = data.Skip(1).ToList();

        await _client.Publish(new PsimData(roomInstance, command, args, isIntro));
    }

}
