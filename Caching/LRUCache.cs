using System.Collections.Generic;
namespace Caching;

public class LRUCache {
    int capacity, numElements;
    CacheNode? head, tail;
    Dictionary<int, CacheNode?> map;

    object locker;

    public LRUCache(int capacity = 32) {
        this.capacity = capacity;
        this.numElements = 0;
        head = tail = null;
        map = new Dictionary<int, CacheNode?>();
        locker = new object();
    }

    public void insert(int key, byte[] value) {
        lock(locker){
            var response = map.GetValueOrDefault(key);
            if(response != null) {
                map[key]!.Value = value;
                moveToHead(response);
                return;
            }
            if(numElements < capacity) {
                head = new CacheNode(key, value, null, head);
                if(head.next != null){
                    head.next.prev = head;
                }
                numElements++;
                if(numElements == 1) {
                    tail = head;
                }
            }
            else {
                var lastItem = map.GetValueOrDefault(tail!.Key);
                map.Remove(lastItem!.Key);
                moveToHead(lastItem);
                head!.Key = key;
                head.Value = value;
            }
            map.Add(key, head);
        }
    }

    public byte[]? get(int key) {
        var item = map.GetValueOrDefault(key);
        lock(locker){
            if(item == null) {
                return null;
            }
            moveToHead(item);
        }
        return item.Value;
    }

    private void moveToHead(CacheNode? node) {
        if(node == head) return;
        if(node == tail){
            if(node!.prev != null){
                node.prev.next = null;
            }
            tail = node.prev;
        }
        else {
            if(node!.prev != null){
                node.prev.next = node.next;
            }
            if(node.next != null){
                node.next.prev = node.prev;
            }
        }
        node.next = head;
        node.prev = null;
        head!.prev = node;
        head = node;
    }
}