using Newtonsoft.Json;
using PsimCsLib.Entities;
using PsimCsLib.Enums;

namespace PsimCsLib.Models;

public class UserDetails
{
	internal class OfflineUserDetailsDto
	{
		public string id { get; set; }
		public string userid { get; set; }
		public string name { get; set; }
		public string avatar { get; set; }
		public string group { get; set; }
		public bool autoconfirmed { get; set; }
		public bool rooms { get; set; }
	}

	internal class UserDetailsDto : OfflineUserDetailsDto
	{
		public new Dictionary<string, UserDetailsRoomDto> rooms { get; set; }

		internal class UserDetailsRoomDto
		{
			public bool isPrivate { get; set; }
			public string p1 { get; set; }
			public string p2 { get; set; }
		}
	}

	public string Id { get; private set; }
	public string UserId { get; private set; }
	public string Name { get; private set; }
	public string Avatar { get; private set; }
	public Rank GlobalRank { get; private set; }
	public bool IsAutoconfirmed { get; private set; }
	public Dictionary<string, Rank> Rooms { get; private set; }

	public UserDetails()
	{

	}

	public static UserDetails? FromJson(string json)
	{
		OfflineUserDetailsDto? dto = null!;
		try
		{
			dto = JsonConvert.DeserializeObject<UserDetailsDto>(json);
		}
		catch
		{
			dto = JsonConvert.DeserializeObject<OfflineUserDetailsDto>(json);
		}

		if (dto == null)
			return null;

		var details = new UserDetails
		{
			Id = dto.id,
			UserId = dto.userid,
			Name = dto.name,
			Avatar = dto.avatar,
			GlobalRank = PsimUsername.GetRank(dto.group),
			IsAutoconfirmed = dto.autoconfirmed,
			Rooms = new Dictionary<string, Rank>()
		};

		if (dto is UserDetailsDto online)
		{
			foreach (var (room, info) in online.rooms)
			{
				try
				{
					var rank = PsimUsername.GetRank(room);
					details.Rooms.Add(room.Substring(1), rank);
				}
				catch
				{
					details.Rooms.Add(room, Rank.Normal);
				}
			}
		}

		return details;
	}
}