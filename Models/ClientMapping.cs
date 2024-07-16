namespace KeycloackTest.Models;

public class RoleMappingDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Composite { get; set; }
    public bool ClientRole { get; set; }
    public string ContainerId { get; set; }
}

public class ClientMappingDTO
{
    public string Id { get; set; }
    public string Client { get; set; }
    public List<RoleMappingDTO> Mappings { get; set; }
}

public class ClientMappingsResponseDTO
{
    public Dictionary<string, ClientMappingDTO> ClientMappings { get; set; }
}


