using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure.Helpers
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}