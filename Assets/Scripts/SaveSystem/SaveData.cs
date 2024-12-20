using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class SaveData
{
    public Dictionary<string, object> data = new Dictionary<string, object>();

    // Универсальный метод для получения значения по ключу
    public T GetData<T>(string key, T defaultValue)
    {
        if (!data.ContainsKey(key))
        {
            // Если ключа нет, сохраняем текущее значение в словарь
            data[key] = defaultValue;
            Debug.LogWarning($"Key '{key}' not found, creating with default value: {defaultValue}");
        }

        object storedValue = data[key];
        Type requestedType = typeof(T);
        Debug.Log(storedValue + " " + requestedType);
        // Преобразуем значение в нужный тип и возвращаем
        try
        {
            if (requestedType == typeof(int))
            {
                // Если запрашиваем тип int, и в данных уже хранится не int
                if (storedValue is int intValue)
                {
                    return (T)(object)intValue;
                }
                return (T)(object)Convert.ToInt32(storedValue);
            }
            else if (requestedType == typeof(float))
            {
                if (storedValue is float floatValue)
                {
                    return (T)(object)floatValue;
                }
                return (T)(object)Convert.ToSingle(storedValue);
            }
            else if (requestedType == typeof(bool))
            {
                if (storedValue is bool boolValue)
                {
                    return (T)(object)boolValue;
                }
                return (T)(object)Convert.ToBoolean(storedValue);
            }
            else if (requestedType == typeof(Voxel[]))
            {
                if (storedValue is Newtonsoft.Json.Linq.JObject jObject)
                {
                    SerializableVoxelArray voxelArray = jObject.ToObject<SerializableVoxelArray>();
                    return (T)(object)voxelArray.ToVoxelArray();
                }
            }
            // Проверка для массивов и списков (generic-тип)
            else if (requestedType.IsArray || (requestedType.IsGenericType && requestedType.GetGenericTypeDefinition() == typeof(List<>)))
            {

                // Преобразуем коллекцию JSON в массив или список
                if (storedValue is Newtonsoft.Json.Linq.JArray jArray)
                {
                    Type elementType = requestedType.IsArray ? requestedType.GetElementType() : requestedType.GetGenericArguments()[0];

                    // Если запрашиваемый тип - массив
                    if (requestedType.IsArray)
                    {
                        // Используем ToObject для конвертации массива элементов
                        var elements = jArray.ToObject(elementType.MakeArrayType());
                        return (T)elements;  // Возвращаем массив
                    }
                    else
                    {
                        // Если запрашиваемый тип - список
                        var list = jArray.ToObject(typeof(List<>).MakeGenericType(elementType));
                        return (T)list;  // Возвращаем список
                    }
                }
            }


            else if (storedValue.GetType() == typeof(Newtonsoft.Json.Linq.JObject))
            {
                Debug.Log("Stored value type: " + storedValue.GetType());
                if (storedValue is Newtonsoft.Json.Linq.JObject jObject)
                {
                    // Преобразуем JObject в нужный тип
                    if ( requestedType == typeof(Color))
                    {
                        // Преобразуем JObject в SerializableColor
                        return (T)(object)jObject.ToObject<SerializableColor>().ToColor();
                    }
                    else if (requestedType == typeof(Vector3))
                    {
                        return (T)(object)jObject.ToObject<SerializableVector3>().ToVector3();

                    }
                    else if (requestedType == typeof(Vector2))
                    {
                        return (T)(object)jObject.ToObject<SerializableVector2>().ToVector2();

                    }
                    else if (requestedType == typeof(Voxel))
                    {
                        return (T)(object)jObject.ToObject<SerializableVoxel>().ToVoxel();
                    }
                }
                
 
            }
            
            T value = (T)data[key];
            return value;
        }
        catch (InvalidCastException ex)
        {
            Debug.LogError($"Error converting data for key '{key}' to type {typeof(T)}: {ex.Message}");
            throw;  // Прокидываем исключение дальше, если преобразование не удалось
        }
    }

    // Метод для установки данных по ключу
    public void SetData<T>(string key, T value)
    {
        Type requestedType = typeof(T);
        Debug.Log(key);

        if (data.ContainsKey(key))
        {

            if (requestedType == typeof(Color))
            {
                Color colorValue = (Color)(object)value;
                data[key] = new SerializableColor(colorValue);

            }
            else if (requestedType == typeof(Vector3))
            {
                Vector3 vector3Value = (Vector3)(object)value;
                data[key] = new SerializableVector3(vector3Value);

            }
            else if (requestedType == typeof(Vector2))
            {
                Vector2 vector2Value = (Vector2)(object)value;
                data[key] = new SerializableVector2(vector2Value);

            }
            else if (requestedType == typeof(Voxel))
            {
                Voxel voxelValue = (Voxel)(object)value;
                data[key] = new SerializableVoxel(voxelValue);
            }
            else if (requestedType == typeof(Voxel[]))
            {
                Debug.Log(requestedType);
                Voxel[] voxelArray = (Voxel[])(object)value;
                data[key] = new SerializableVoxelArray(voxelArray);
            }
            else
                data[key] = value;
        }
        else
        {
            if (requestedType == typeof(Color))
            {
                Color colorValue = (Color)(object)value;
                data.Add(key, new SerializableColor(colorValue));

            }
            else if (requestedType == typeof(Vector3))
            {
                Vector3 vector3Value = (Vector3)(object)value;
                data.Add(key, new SerializableVector3(vector3Value));

            }
            else if (requestedType == typeof(Vector2))
            {
                Vector2 vector2Value = (Vector2)(object)value;
                data.Add(key, new SerializableVector2(vector2Value));

            }
            else if (requestedType == typeof(Voxel))
            {
                Voxel voxelValue = (Voxel)(object)value;
                data.Add(key, new SerializableVoxel(voxelValue));
            }
            else if (requestedType == typeof(Voxel[]))
            {
                Debug.Log(requestedType + "have");

                Voxel[] voxelArray = (Voxel[])(object)value;
                data.Add(key, new SerializableVoxelArray(voxelArray));
            }
            else
                data.Add(key, value);
        }
    }

}

[Serializable]
public class SerializableColor
{
    public float r;
    public float g;
    public float b;
    public float a;

    public SerializableColor() { }

    public SerializableColor(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }

    public Color ToColor()
    {
        return new Color(r, g, b, a);
    }
}
[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3() { }

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
[Serializable]
public class SerializableVector2
{
    public float x;
    public float y;

    public SerializableVector2() { }

    public SerializableVector2(Vector2 vector)
    {
        x = vector.x;
        y = vector.y;
    }

    public Vector2 ToVector2()
    {
        return new Vector3(x, y);
    }

}
[Serializable]
public class SerializableVoxel
{
    public int x;
    public int y;
    public int z;

    public float r;
    public float g;
    public float b;

    public bool isActive;

    public SerializableVoxel() { }

    public SerializableVoxel(Voxel voxel)
    {
        x = voxel.X;
        y = voxel.Y;
        z = voxel.Z;

        r = voxel.R;
        g = voxel.G;
        b = voxel.B;
        isActive = voxel.IsActive;
    }

    public Voxel ToVoxel()
    {
        return new Voxel(new Vector3(x, y, z), new Color(r, g, b), isActive);
    }
}
[Serializable]
public class SerializableVoxelArray
{
    public SerializableVoxel[] voxels;

    public SerializableVoxelArray() { }

    public SerializableVoxelArray(Voxel[] voxelArray)
    {
        voxels = new SerializableVoxel[voxelArray.Length];
        for (int i = 0; i < voxelArray.Length; i++)
        {
            voxels[i] = new SerializableVoxel(voxelArray[i]);
        }
    }

    public Voxel[] ToVoxelArray()
    {
        Voxel[] voxelArray = new Voxel[voxels.Length];
        for (int i = 0; i < voxels.Length; i++)
        {
            voxelArray[i] = voxels[i].ToVoxel();
        }
        return voxelArray;
    }
}
