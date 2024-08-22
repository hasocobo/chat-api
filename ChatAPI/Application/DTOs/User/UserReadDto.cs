namespace ChatAPI.Application.DTOs.User;

public class UserReadDto
{
  public int Id { get; set; }
  public required string Username { get; set; }

  public List<ConnectionId> ConnectionIds { get; set; } = [];

}

public class ConnectionId 
{
  public int Id { get; set; }
  public required string Connection { get; set; }
}