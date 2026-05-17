using UnityEngine;
using System.Security.Cryptography; // Required for true secure random generation

public struct SecureInt
{
    // Hidden scrambled value
    private int encryptedValue;
    
    // Secret key used to scramble the data
    private int key;

    // Setup for the secure integer
    public SecureInt(int initialValue)
    {
        // Creates a cryptographically secure random key
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            // Creates an empty 4-byte array 
            byte[] bytes = new byte[4]; 
            
            // Fills the array with random, unpredictable bytes
            rng.GetBytes(bytes); 
            
            // Converts those random bytes into the integer key
            key = System.BitConverter.ToInt32(bytes, 0); 
        }
        
        // Scrambles the starting value with the key
        encryptedValue = initialValue ^ key;
    }

    // Unscrambles the value to read it
    public int GetValue()
    {
        return encryptedValue ^ key;
    }

    // Lets unity treat this exactly like a normal integer
    public static implicit operator SecureInt(int value)
    {
        return new SecureInt(value);
    }

    // Converts back to a normal integer when needed
    public static implicit operator int(SecureInt secureInt)
    {
        return secureInt.GetValue();
    }

    // Required override for displaying text on the screen
    public override string ToString()
    {
        return GetValue().ToString();
    }
}
