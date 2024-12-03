using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst { get; private set; }

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else if (Inst != this)
        {
            Destroy(gameObject); // 기존 인스턴스가 있다면 새로운 것을 파괴
        }
    }

    /// <summary>
    /// 제공된 데이터를 경로에 JSON 파일로 저장
    /// </summary>
    /// <param name="data"> 저장할 데이터 객체 </param>
    /// <param name="filePath"> 데이터를 저장할 경로 </param>
    public void SaveData<T>(T data, string filePath) where T : class, new()
    {
        // 데이터 설정이 안되있으면 디폴트 데이터 저장
        if (data == null)
        {
            data = new T(); // 기본 생성자 호출
        }

        string json = JsonUtility.ToJson(data, true); // JSON 문자열로 변환
        try
        {
            File.WriteAllText(filePath, json);
            Debug.Log($"{typeof(T).Name} 저장 성공: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"{typeof(T).Name} 저장 실패: {e.Message}");
        }
    }

    /// <summary>
    /// 경로에 저장된 JSON 파일을 다시 데이터 객체로 변환 후, 반환한다.
    /// </summary>
    /// <param name="filePath"> 데이터가 저장된 경로 </param>
    public T LoadData<T>(string filePath) where T : class, new()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"{typeof(T).Name} 저장 파일이 없습니다. 새 데이터 생성.");
            return new T(); // 파일이 없으면 기본 생성자를 사용해 새 데이터 생성
        }

        try
        {
            string json = File.ReadAllText(filePath);   // text 모두 읽기
            T data = JsonUtility.FromJson<T>(json);     // 데이터 객체 변환
            Debug.Log($"{typeof(T).Name} 불러오기 성공: {filePath}");
            return data;    // 데이터 객체 반환
        }
        catch (Exception e)
        {
            Debug.LogError($"{typeof(T).Name} 불러오기 실패: {e.Message}");
            return new T(); // 오류 발생 시 디폴트 데이터 생성
        }
    }

    /// <summary>
    /// 경로에 저장된 JSON 파일을 삭제한다.
    /// </summary>
    /// <param name="filePath"> 데이터가 저장된 경로 </param>
    public void DeleteData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"파일이 존재하지 않습니다: {filePath}");
            return;
        }

        try
        {
            File.Delete(filePath);
            Debug.Log($"파일이 삭제되었습니다: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"파일 삭제 실패: {e.Message}");
        }
    }

}
