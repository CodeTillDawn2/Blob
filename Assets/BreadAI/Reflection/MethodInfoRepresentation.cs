public class MethodInfoRepresentation
{
    public string Name { get; set; }
    public string AssemblyName { get; set; }
    public string ClassName { get; set; }
    public string Signature { get; set; }
    public string Signature2 { get; set; }
    public int MemberType { get; set; }
    public object[] GenericArguments { get; set; } // or specific type, based on your needs
}