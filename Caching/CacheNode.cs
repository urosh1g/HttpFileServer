namespace Caching;

public class CacheNode {
    public int Key { get; set; }
    public byte[]? Value { get; set; }
    public CacheNode? prev, next;

    public CacheNode(int key, byte[]? value, CacheNode? prev, CacheNode? next){
        this.Key = key;
        this.Value = value;
        this.prev = prev;
        this.next = next;
    }
}