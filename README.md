# AI Finance Tracker — Capstone Project Source Code

This repository contains the complete source code for the AI-Powered Personal Finance Tracker, developed for the ITSS-440 Capstone Project.  
The application is built with Blazor WebAssembly and runs entirely in the browser, storing all data locally for user privacy.

---

## How to Run the Application (Visual Studio 2022)

Follow these instructions to download, open, and run the project.

---

## 1. Install Required Software

- **.NET 8 SDK**  
  https://dotnet.microsoft.com/en-us/download/dotnet/8.0

- **Visual Studio 2022**  
  Required workload: **ASP.NET and Web Development**

- **Git (optional)**  
  https://git-scm.com/

---

## 2. Clone the Repository

### Option A — Clone Using Visual Studio

1. Open **Visual Studio 2022**
2. Select **Clone a repository**
3. Enter your repository URL  
4. Click **Clone**

### Option B — Clone Using Git

```
git clone https://github.com/<your-username>/<your-repo>.git
```

---

## 3. Open the Project in Visual Studio

1. Open the solution file:

```
FinanceTracker.sln
```

2. Allow Visual Studio to restore NuGet packages  
3. Ensure build settings:  
   - **Debug** configuration  
   - **Any CPU** platform  

---

## 4. Run the Application

1. Select the Blazor WebAssembly project:

```
FinanceTracker
```

2. Press **F5** to start debugging  
3. Your browser will open automatically at a URL such as:

```
https://localhost:xxxx/
```

The application runs fully client-side using WebAssembly.

---

## Project Summary

- Built with **Blazor WebAssembly**
- Client-side only — no backend required
- Local transaction storage via browser localStorage
- Features:
  - Dashboard overview
  - Transaction management
  - Add transaction form
  - Dark mode support
  - Collapsible sidebar navigation
  - AI-generated financial insights
