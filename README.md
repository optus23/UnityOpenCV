# Unity OpenCV Integration

[![Unity Version](https://img.shields.io/badge/Unity-2020.x%2B-blue)](https://unity.com)
[![OpenCV Version](https://img.shields.io/badge/OpenCV-4.11.0-green)](https://opencv.org)

> **Integrate** OpenCV’s C API into Unity using P/Invoke and a precompiled `opencv_world4110.dll`, with no custom native wrappers.

---

## Overview

This repository demonstrates how to use OpenCV’s **imgproc** module directly in Unity via P/Invoke. It includes reusable C# scripts for image processing tasks such as blurring, color conversion, geometric transforms, and drawing operations — all powered by a precompiled `opencv_world4110.dll`.

---

## Features

* **Gaussian Blur** (`cvSmooth`)
* **Grayscale Conversion** (`cvCvtColor`)
* **Static Four-Color Gradient** generation
* **Rotation & Scaling** with custom affine matrix (`cvWarpAffine`)
* **Rectangle Outline Drawing** (`cvRectangle`)
* **Timestamped Image Export** to PNG
* **UI Utilities**: Label timer, entry animations, hover handlers

---

## Requirements

* **Unity**: 2020.x or later
* **Platform**: Windows Standalone (x86\_64)
* **DLL**: OpenCV 4.11.0 `opencv_world4110.dll`

---

## Setup

1. **Clone** this repo under your Unity project’s `Assets/` directory:

   ```bash
   cd <YourUnityProject>/Assets
   git clone https://github.com/optus23/UnityOpenCV.git OpenCV
   ```
2. **Place** `opencv_world4110.dll` into:

   ```
   Assets/Plugins/x86_64/opencv_world4110.dll
   ```
3. **Configure** plugin importer (select the DLL):

   * Uncheck **Any Platform**
   * Check **Windows Editor** and **Standalone**
   * Set **CPU** to **x86\_64**
4. **Open** or create a Unity scene and proceed to [Usage](#usage).

---

## Project Structure

```
Assets/Scripts/

├─ OpenCV/Internal
    ├─ CvConstants.cs           # Constant definitions & enums
    ├─ OpenCvNativeImporter.cs  # [DllImport] declarations
│├─ OpenCV/Runtime
    ├─ OpenCvManager.cs         # Manages textures & RawImage refs
│   ├─ GaussianBlurFilter.cs
│   ├─ GreyscaleFilter.cs
│   ├─ OutlineImageFilter.cs
│   ├─ RotateImageFilter.cs
│   └─ GradientBackground.cs
│
├─ UI
│   ├─ ButtonHoverHandler.cs
│   └─ UIEnterAnimation.cs
│   └─ UILabelInteraction.cs

├─ Application
│   ├─ ApplicationManager.cs
│   ├─ ImageSaver.cs
│   └─ OpenLink.cs
```

---

## Usage

### 1. Prepare UI

* **Canvas** → **Raw Image** (`CanvasImage`) for displaying processed textures.
* Optional **Raw Image** (`BackgroundImage`) for gradient background.
* **GameObject** + **OpenCvManager**: assign `SourceTexture`, `CanvasImage`, and `BackgroundImage`.

### 2. Add Filters

* **Gaussian Blur**: Button → `GaussianBlurFilter.ApplyGaussianBlur()`
* **Grayscale**: Button → `GreyscaleFilter.ApplyGrayscale()`
* **Rotate**: Button → `RotateImageFilter.ApplyRotation()`
* **Outline**: Button → `OutlineImageFilter.ApplyOutline()`
* **Gradient**: Handled automatically by `GradientBackground` script
* **Save Image**: Button → `ImageSaver.SaveImageToDisk()`

### 3. Export Images

Processed images are saved as:

```
image_YYYY-MM-DD_HH-mm-ss.png
```

in the build’s root folder (next to the `.exe`).

---

## Building

1. **File → Build Settings**
2. Platform: **PC, Mac & Linux Standalone** → **Windows**
3. Architecture: **x86\_64**
4. Ensure `Assets/Plugins/x86_64/opencv_world4110.dll` is included
5. Click **Build and Run**

---

## Challenges & Solutions

During development, three major challenges appeared:

DLL Loading & Architecture MismatchAt first, Unity threw DllNotFoundException because it could not locate opencv_world4110.dll. By default, Unity did not include the native DLL in the build output. To resolve this, the DLL was placed under Assets/Plugins/x86_64/, the importer settings were adjusted (unchecked Any Platform, enabled Windows Editor and Standalone, CPU set to x86_64), and a manual copy step was added to the packaged build. This ensured the DLL was available in UnityNativeOpenCV_Data/Plugins/x86_64/ at runtime.

P/Invoke EntryPointNotFoundCertain intended C API functions—most notably cvGetRotationMatrix2D—were not exposed by the precompiled opencv_world4110.dll. Instead of building a custom native wrapper, the solution was to hand-calculate the 2×3 affine transformation matrix in C# (using Mathf.Cos/Sin for rotation + scale parameters) and feed it into cvWarpAffine. This avoided missing exports and kept all logic in managed code.

Contour Filter DeferredAn ambitious plan to implement a full contour detection and analysis pipeline (using cvFindContours, cvContourArea, cvArcLength, cvApproxPoly, etc.) was started. However, the complexity of marshaling sequences and managing memory within the time constraints proved prohibitive. As a result, contour-based filtering was postponed for a future iteration.

These targeted solutions allowed stable, performant integration of OpenCV’s C API into Unity, entirely in C#, without recourse to custom native code. stable, performant OpenCV interop entirely within C# and without custom native wrappers.


---

## License

MIT License. See [LICENSE](LICENSE).

---

## Credits

**Marc Galvez Llorens**
🔗 [LinkedIn](https://www.linkedin.com/in/marc-g%C3%A1lvez-llorens/)

Built by Marc while completing the UnityOpenCV test. Contributions welcome at [https://github.com/optus23/UnityOpenCV](https://github.com/optus23/UnityOpenCV)
