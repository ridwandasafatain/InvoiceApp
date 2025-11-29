# TechTest

---

# 1. Statement Test — Code Analysis & Optimization

## **1.1. Performance Issue Explanation**

The main performance problems in the original implementation are:

### **O(n²) time complexity**
Because of the nested loop (`for i × for j`), the function checks every number against every other number.

### **Produces duplicate pairs**
Example: both `(2,3)` and `(3,2)` are added.

### **Redundant comparisons**
The code compares `i` and `j` even when the values have already been validated as a pair.

---

## **1.2. Optimized Version (Improved to O(n))**

This solution uses a `HashSet` to check complementary values in constant time, and a second `HashSet` to prevent duplicate pairs.

```csharp
public List<(int, int)> FindPairs(List<int> numbers, int target)
{
    var result = new List<(int, int)>();
    var seen = new HashSet<int>();
    var added = new HashSet<string>();

    foreach (var num in numbers)
    {
        int needed = target - num;

        if (seen.Contains(needed))
        {
            int a = Math.Min(num, needed);
            int b = Math.Max(num, needed);
            string key = $"{a}:{b}";

            if (!added.Contains(key))
            {
                result.Add((a, b));
                added.Add(key);
            }
        }

        seen.Add(num);
    }

    return result;
}
```

---

## **1.3. How duplicates are avoided**

 **Sorting each pair before adding**  
Ensures `(2,3)` and `(3,2)` are always represented as `(2,3)`.

 **Using a HashSet of keys**  
Keys like `"2:3"` uniquely identify a pair and avoid duplication.

---

# 2. Logical Test — Concurrency & Thread Safety

## **2.1. Potential multi-threading problems**

If multiple threads access the class simultaneously:

### **Race conditions**
`counter++` is not atomic.

### **Lost updates**
Two increments may result in only one stored.

### **Inconsistent final results**
Expected 100 increments may end up lower.

---

## **2.2. Thread-safe version using Interlocked**

```csharp
public class CounterService
{
    private int counter = 0;

    public void Increment()
    {
        Interlocked.Increment(ref counter);
    }

    public void Decrement()
    {
        Interlocked.Decrement(ref counter);
    }

    public int GetValue()
    {
        return Interlocked.CompareExchange(ref counter, 0, 0);
    }
}
```

---

## **2.3. Distributed environment considerations**

In a distributed system, shared memory cannot be used. Solutions include:

### **Centralized shared counter**
- Redis `INCR`
- Database row-level locking
- Event sourcing

### **Optimistic concurrency**
Using version tokens (rowversion / ETag).

### **Distributed locks**
- Redis RedLock
- Consul locks
- Zookeeper locks

---

##

# 3.  Data Integrity & UI Test - Transactional Consistency with Visualization
### **Clone and run this repository to see the result**

### **Validation for Preventing future inconsistencies (Database Level)**
```sql
CREATE TRIGGER trg_UpdateInvoiceTotal
ON InvoiceDetail
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Invoice
    SET TotalAmount = (
        SELECT ISNULL(SUM(Qty * Price), 0)
        FROM InvoiceDetail
        WHERE InvoiceDetail.InvoiceID = Invoice.InvoiceID
    )
    WHERE Invoice.InvoiceID IN (
        SELECT InvoiceID FROM inserted
        UNION
        SELECT InvoiceID FROM deleted
    );
END
```
###
