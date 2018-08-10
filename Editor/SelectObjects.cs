using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace gamesolids
{
    [ExecuteInEditMode]
    [CanEditMultipleObjects]
    public static class SelectObjects
    {
        public static Type selectedType;
        public static string selectedQuery;
        public static float selectedAngle;
        public static float selectedVector;
        public static float selectedDistance;


        /// <summary>
        /// Gets list of all Object Types associated with active object.
        /// </summary>
        /// <returns>array of Types</returns>
        public static List<Type> GetCurrentGameObjectTypes()
        {
            List<Type> comps = new List<Type>();
            foreach(Component comp in Selection.activeGameObject.GetComponents(typeof(Component)))
            {
                Type tComp = comp.GetType();
                comps.Add(tComp);
            }
            return comps;
        }

        /// <summary>
        /// Finds all the objects in scene that match the query type. 
        /// The selection is filtered based on the 'ignore' criteria.
        /// <param name="layers">array of layers to ignore</param>
        /// <param name="tags">array of tags to ignore</param>
        /// <param name="ptags">array of tags parents have, so ignore the children</param>
        /// <returns> Array of GameObjects found or null </returns>
        public static GameObject[] GetTypedObjectsInScene(int[] layers, string[] tags, string[] ptags)
        {
            List<UnityEngine.GameObject> objectsInScene = new List<UnityEngine.GameObject>();
            
            //select all objects of selected type
            Type T = SelectObjects.selectedType;

            foreach (GameObject go in (GameObject[]) SceneView.FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                
                if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
                    continue;
                
                if (Array.Exists(layers, element => element == go.layer))
                    continue;

                if (Array.Exists(tags, element => element == go.tag))
                    continue;

                //if any parents have a childtag??
                Transform t = go.transform;
                while (t.parent != null)
                {
                    if (Array.Exists(ptags, element => element == t.parent.tag))
                    {
                        break;
                    }
                    t = t.parent.transform;
                }
                if (t.parent != null)
                    continue;

                if (go.GetComponent(T))
                    objectsInScene.Add(go);
            }

            Selection.objects = objectsInScene.ToArray();
            SceneView.lastActiveSceneView.Repaint();
            return Selection.objects as GameObject[];
        }

        
        /// <summary>
        /// Finds all the objects in scene that match the query string. 
        /// The selection is filtered based on the 'ignore' criteria.
        /// <param name="layers">array of layers to ignore</param>
        /// <param name="tags">array of tags to ignore</param>
        /// <param name="ptags">array of tags parents have, so ignore the children</param>
        /// <returns> Array of GameObjects found or null </returns>
        public static GameObject[] GetNamedObjectsInScene(int[] layers, string[] tags, string[] ptags)
        {
            List<UnityEngine.GameObject> objectsInScene = new List<UnityEngine.GameObject>();

            //select all objects of selected type

            foreach (GameObject go in (GameObject[])SceneView.FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {

                if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
                    continue;

                if (Array.Exists(layers, element => element == go.layer))
                    continue;

                if (Array.Exists(tags, element => element == go.tag))
                    continue;

                //if any parents have a childtag??
                Transform t = go.transform;
                while (t.parent != null)
                {
                    if (Array.Exists(ptags, element => element == t.parent.tag))
                    {
                        break;
                    }
                    t = t.parent.transform;
                }
                if (t.parent != null)
                    continue;

                if (go.name.Equals(SelectObjects.selectedQuery))
                    objectsInScene.Add(go);

            }

            Selection.objects = objectsInScene.ToArray();
            SceneView.lastActiveSceneView.Repaint();
            return Selection.objects as GameObject[];
        }


        /// <summary>
        /// Finds all the objects in scene that Contain the query string. 
        /// The selection is filtered based on the 'ignore' criteria.
        /// <param name="layers">array of layers to ignore</param>
        /// <param name="tags">array of tags to ignore</param>
        /// <param name="ptags">array of tags parents have, so ignore the children</param>
        /// <returns> Array of GameObjects found or null </returns>
        public static GameObject[] GetLikeObjectsInScene(int[] layers, string[] tags, string[] ptags)
        {
            List<UnityEngine.GameObject> objectsInScene = new List<UnityEngine.GameObject>();

            //select all objects of selected type

            foreach (GameObject go in (GameObject[])SceneView.FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {

                if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
                    continue;

                if (Array.Exists(layers, element => element == go.layer))
                    continue;

                if (Array.Exists(tags, element => element == go.tag))
                    continue;

                //if any parents have a childtag??
                Transform t = go.transform;
                while (t.parent != null)
                {
                    if (Array.Exists(ptags, element => element == t.parent.tag))
                    {
                        break;
                    }
                    t = t.parent.transform;
                }
                if (t.parent != null)
                    continue;

                if (go.name.Contains(SelectObjects.selectedQuery))
                    objectsInScene.Add(go);

            }

            Selection.objects = objectsInScene.ToArray();
            SceneView.lastActiveSceneView.Repaint();
            return Selection.objects as GameObject[];
        }



        /// <summary>
        /// Finds all the objects in scene that are in range of the any of the objects angles. 
        /// The selection is filtered based on the 'ignore' criteria.
        /// <param name="layers">array of layers to ignore</param>
        /// <param name="tags">array of tags to ignore</param>
        /// <param name="ptags">array of tags parents have, so ignore the children</param>
        /// <returns> Array of GameObjects found or null </returns>
        public static GameObject[] GetAngleObjectsInScene(int[] layers, string[] tags, string[] ptags)
        {
            List<UnityEngine.GameObject> objectsInScene = new List<UnityEngine.GameObject>();

            //select all objects of selected type

            foreach (GameObject go in (GameObject[])SceneView.FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {

                if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
                    continue;

                if (Array.Exists(layers, element => element == go.layer))
                    continue;

                if (Array.Exists(tags, element => element == go.tag))
                    continue;

                //if any parents have a childtag??
                Transform t = go.transform;
                while (t.parent != null)
                {
                    if (Array.Exists(ptags, element => element == t.parent.tag))
                    {
                        break;
                    }
                    t = t.parent.transform;
                }
                if (t.parent != null)
                    continue;

                if (Vector3.Angle(go.transform.eulerAngles, Selection.activeGameObject.transform.eulerAngles) <= SelectObjects.selectedAngle)
                    objectsInScene.Add(go);
                
            }

            Selection.objects = objectsInScene.ToArray();
            SceneView.lastActiveSceneView.Repaint();
            return Selection.objects as GameObject[];
        }


        /// <summary>
        /// Finds all the objects in scene that are near the directional Vector.
        /// The selection is filtered based on the 'ignore' criteria.
        /// <param name="layers">array of layers to ignore</param>
        /// <param name="tags">array of tags to ignore</param>
        /// <param name="ptags">array of tags parents have, so ignore the children</param>
        /// <returns> Array of GameObjects found or null </returns>
        public static GameObject[] GetVectorObjectsInScene(int[] layers, string[] tags, string[] ptags)
        {
            List<UnityEngine.GameObject> objectsInScene = new List<UnityEngine.GameObject>();

            //select all objects of selected type

            foreach (GameObject go in (GameObject[])SceneView.FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {

                if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
                    continue;

                if (Array.Exists(layers, element => element == go.layer))
                    continue;

                if (Array.Exists(tags, element => element == go.tag))
                    continue;

                //if any parents have a childtag??
                Transform t = go.transform;
                while (t.parent != null)
                {
                    if (Array.Exists(ptags, element => element == t.parent.tag))
                    {
                        break;
                    }
                    t = t.parent.transform;
                }
                if (t.parent != null)
                    continue;

                if (Vector3.Distance(go.transform.eulerAngles, Selection.activeGameObject.transform.eulerAngles) <= SelectObjects.selectedAngle)
                    objectsInScene.Add(go);
                
            }

            Selection.objects = objectsInScene.ToArray();
            SceneView.lastActiveSceneView.Repaint();
            return Selection.objects as GameObject[];
        }

        /// <summary>
        /// Finds all the objects in scene that are near the location.
        /// The selection is filtered based on the 'ignore' criteria.
        /// <param name="layers">array of layers to ignore</param>
        /// <param name="tags">array of tags to ignore</param>
        /// <param name="ptags">array of tags parents have, so ignore the children</param>
        /// <returns> Array of GameObjects found or null </returns>
        public static GameObject[] GetDistanceObjectsInScene(int[] layers, string[] tags, string[] ptags)
        {
            List<UnityEngine.GameObject> objectsInScene = new List<UnityEngine.GameObject>();

            //select all objects of selected type

            foreach (GameObject go in (GameObject[])SceneView.FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {

                if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
                    continue;

                if (Array.Exists(layers, element => element == go.layer))
                    continue;

                if (Array.Exists(tags, element => element == go.tag))
                    continue;

                //if any parents have a childtag??
                Transform t = go.transform;
                while (t.parent != null)
                {
                    if (Array.Exists(ptags, element => element == t.parent.tag))
                    {
                        break;
                    }
                    t = t.parent.transform;
                }
                if (t.parent != null)
                    continue;

                Debug.Log(Selection.activeGameObject);
                if (Vector3.Distance(go.transform.position, Selection.activeGameObject.transform.position) <= SelectObjects.selectedDistance)
                    objectsInScene.Add(go);
                
            }

            Selection.objects = objectsInScene.ToArray();
            SceneView.lastActiveSceneView.Repaint();
            return Selection.objects as GameObject[];
        }
    }
}