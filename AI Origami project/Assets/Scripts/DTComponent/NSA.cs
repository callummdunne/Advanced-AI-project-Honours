using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NSA : MonoBehaviour
{
    //// T Cell Inspired Algorithm
    //// NS Algorithm: Simple Greedy NSA

    // initial area threshold
    private static int athreshold = 10;

    // Total area of 10,000 (100x100)
    private int XSpace = 100;
    private int YSpace = 100;

    // Non-Self Initial Detectors
    private Detector[,] initialDetectors;
    // Non-Self Final Detectors
    private static List<Detector> finalDetectors = new List<Detector>();

    // Origami space
    public static Detector SelfSpace = new Detector();
    // Self Space width and height
    int Width = 20;
    int Height = 20;

    public void Start()
    {
        print("Negative Selection Algorithm");
        print("==========================================================================");

        // create a 10 by 10 array of detectors 
        initialDetectors = new Detector[XSpace / athreshold, YSpace / athreshold];

        // Self Cells Space
        SelfSpace.topleftX = GetRandomNumber(0, XSpace - Width);
        SelfSpace.topleftY = GetRandomNumber(YSpace, 0 + Height);
        SelfSpace.bottomrightX = SelfSpace.topleftX + Width;
        SelfSpace.bottomrightY = SelfSpace.topleftY - Height;
        print("===== Self Space");
        SelfSpace.PRINT();

        // Step 1
        GenerateInitialDetectors();
        // Step 2
        RemoveSelfDetectors();
        // Step 3
        print("===== Non Self Detectors");
        JoinDetectors();
        // Step 4
        PopulateListOfDetectors();
        print("==========================================================================");
    }

    public void Update()
    {

    }

    // Detector Rule
    public class Detector
    {
        public int topleftX;
        public int topleftY;
        public int bottomrightX;
        public int bottomrightY;
        public bool isDetector = true;

        public void PRINT()
        {
            print("TopLeftPoint = " + topleftX + ":" + topleftY + " BottomRightPoint = " + bottomrightX + ":" + bottomrightY);
        }
    }

    // Step 1
    private void GenerateInitialDetectors()
    {
        int detectorCount = 0;
        for(int c = 0; c < YSpace / athreshold; c++)
        {
            for(int i = 0; i < XSpace / athreshold; i++)
            {
                Detector detector = new Detector();
                detector.topleftX = (i) * athreshold;
                detector.topleftY = (c + 1) * athreshold;
                detector.bottomrightX = (i + 1) * athreshold;
                detector.bottomrightY = (c) * athreshold;
                initialDetectors[i, c] = detector;
                detectorCount++;
            }
        }
    }

    // Step 2
    private void RemoveSelfDetectors()
    {
        int numSelfDetectors = 0;
        for(int c = 0; c < YSpace / athreshold; c++)
        {
            for(int i = 0; i < XSpace / athreshold; i++)
            {
                Detector detector = initialDetectors[i, c];
                if(CheckDetector(SelfSpace, detector))
                {
                    initialDetectors[i, c] = null;
                    numSelfDetectors++;
                }
            }
        }
    }

    public static bool CheckDetector(Detector d1, Detector d2)
    {
        int t1x1 = d1.topleftX;
        int t1y1 = d1.bottomrightY;
        int t1x2 = d1.bottomrightX;
        int t1y2 = d1.topleftY;
        int t2x1 = d2.topleftX;
        int t2y1 = d2.bottomrightY;
        int t2x2 = d2.bottomrightX;
        int t2y2 = d2.topleftY;
        return (t2x2 >= t1x1 && t2x1 <= t1x2) && (t2y2 >= t1y1 && t2y1 <= t1y2);
    }

    public static bool CheckPoint(Detector d1,Point p)
    {
        Detector d2 = new Detector
        {
            topleftX = p.X,
            topleftY = p.Y,
            bottomrightX = p.X,
            bottomrightY = p.Y
        };
        return CheckDetector(d1, d2);
    }

    // Step 3
    private void JoinDetectors()
    {
        // Phase 1: Horizontial Combination
        for(int y = 0; y < YSpace / athreshold; y++)
        {
            for(int x = 0; x < ((XSpace / athreshold) - 1); x++)
            {
                Detector detector1 = initialDetectors[x, y];
                Detector detector2 = initialDetectors[x + 1, y];
                if(detector1 != null && detector2 != null)
                {
                    if(detector1.bottomrightX == detector2.topleftX)
                    {
                        detector2.topleftX = detector1.topleftX;
                        detector2.topleftY = detector1.topleftY;
                        initialDetectors[x, y] = null;
                        initialDetectors[x + 1, y] = detector2;
                    }
                }
            }
        }

        // Phase 2: Vertical Combination
        for(int x = (XSpace / athreshold - 1); x >= 0; x--)
        {
            for(int y = 0; y < ((YSpace / athreshold) - 1); y++)
            {
                Detector detector1 = initialDetectors[x, y];
                Detector detector2 = initialDetectors[x, y + 1];
                if(detector1 != null && detector2 != null)
                {
                    if(detector1.topleftY == detector2.bottomrightY && detector1.topleftX == detector2.topleftX)
                    {
                        detector2.bottomrightX = detector1.bottomrightX;
                        detector2.bottomrightY = detector1.bottomrightY;
                        initialDetectors[x, y] = null;
                        initialDetectors[x, y + 1] = detector2;
                    }
                }
            }
        }
    }

    // Step 4
    public void PopulateListOfDetectors()
    {
        for(int i = 0; i < XSpace / athreshold; i++)
        {
            for(int c = 0; c < (YSpace / athreshold); c++)
            {
                Detector detector = initialDetectors[i, c];
                if(detector != null)
                {
                    finalDetectors.Add(detector);
                    detector.PRINT();
                }
            }
        }
    }

    public int GetRandomNumber(int min, int max)
    {
        return ((int)(Random.Range(min, max) / 10)) * 10;
    }

    public static Point GenSelfCell()
    {
        Point p = new Point();
        p.X = Random.Range(SelfSpace.topleftX, SelfSpace.bottomrightX);
        p.Y = Random.Range(SelfSpace.bottomrightY, SelfSpace.topleftY);
        return p;
    }

    public static Point GenNonSelfCell()
    {
        foreach(Detector d in finalDetectors)
        {
            Point p = new Point();
            p.X = Random.Range(d.topleftX, d.bottomrightX);
            p.Y = Random.Range(d.bottomrightY, d.topleftY);
            return p;
        }
        return null;
    }

    public static bool CheckIfSelfCell(Point p)
    {
        return CheckPoint(SelfSpace, p);
    }
}
