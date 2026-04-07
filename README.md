# 🌱 Green Footprint Stateful
### Assignment 5/6 — Web Application ReadMe
**CSE 445 — Distributed Software Development**
*Person A (Pink) • Person B (Purple)*

---

## 1. System Description

Green Footprint Stateful is an environmental awareness web application that encourages sustainable living by tracking users' green and non-green daily actions. The application integrates real-time weather data, soil analysis, and a stateful green footprint scoring service to provide users with a personalized environmental dashboard.

The application is built on the **ASP.NET Web Application framework** (not ASP.NET Core) using a 4-tier architecture as required by the assignment specification.

---

## 2. 4-Tier Architecture

### Presentation Layer (Default.aspx)
The central hub of the application. `Default.aspx` hosts all TryIt interfaces, the service directory table, and connects to all services and local components. Users interact through web forms to invoke services and view results.

### Application Logic / Service Layer
- `GreenFootprint.asmx` — Stateful WSDL service with `AddAction` and `GetScore` operations
- Weather Service — WSDL service returning 5-day forecast by ZIP code (NOAA NDFD)
- Soil Data Service — RESTful service using USDA NRCS Soil Data Access API

### Local Component Layer
- `Global.asax` — `Application_Start` initializes data store; `Session_Start` logs new sessions
- HashPassword DLL — Local SHA-256 hashing class library for secure password storage
- Session State — Tracks active user score and state level across interactions
- `users.json` — Persistent JSON data store in `App_Data` for user credentials and scores

### Data Layer
- `App_Data/users.json` — stores name, hashed password, current score, and green state
- Session variables — ephemeral score/state tracking during an active visit

---

## 3. Project File Structure

```
Assignment 5/
├── App_Data/
│   └── users.json                  ← Persistent user data store
├── bin/
│   ├── roslyn/                     ← Compiler support DLLs
│   │   ├── csc.exe
│   │   ├── csc.exe.config
│   │   ├── csc.rsp
│   │   ├── csi.exe
│   │   ├── csi.exe.config
│   │   ├── csi.rsp
│   │   ├── Microsoft.Build.Tasks.CodeAnalysis.dll
│   │   ├── Microsoft.CodeAnalysis.CSharp.dll
│   │   ├── Microsoft.CodeAnalysis.CSharp.Scripting.dll
│   │   ├── Microsoft.CodeAnalysis.dll
│   │   ├── Microsoft.CodeAnalysis.Scripting.dll
│   │   ├── Microsoft.CSharp.dll
│   │   ├── Microsoft.DiaSymReader.dll
│   │   ├── Microsoft.DiaSymReader.Native.amd64.dll
│   │   ├── Microsoft.Managed.Compiler.dll
│   │   ├── Microsoft.VisualBasic.dll
│   │   ├── Microsoft.Win32.Primitives.dll
│   │   ├── System.AppContext.dll
│   │   ├── System.Collections.dll
│   │   ├── System.Console.dll
│   │   ├── System.Diagnostics.Debug.dll
│   │   ├── System.Diagnostics.FileVersionInfo.dll
│   │   ├── System.Diagnostics.StackTrace.dll
│   │   ├── System.Globalization.dll
│   │   ├── System.IO.Compression.dll
│   │   ├── System.IO.Compression.ZipFile.dll
│   │   ├── System.IO.FileSystem.dll
│   │   ├── System.IO.FileSystem.Primitives.dll
│   │   ├── System.Net.Http.dll
│   │   ├── System.Net.Sockets.dll
│   │   ├── System.Reflection.dll
│   │   ├── System.Runtime.InteropServices.dll
│   │   ├── System.Security.Claims.dll
│   │   ├── System.Security.Cryptography.Algorithms.dll
│   │   ├── System.Security.Cryptography.Encoding.dll
│   │   ├── System.Security.Cryptography.Primitives.dll
│   │   ├── System.Text.Encoding.dll
│   │   ├── System.Threading.dll
│   │   ├── System.ValueTuple.dll
│   │   ├── System.Xml.ReaderWriter.dll
│   │   ├── System.Xml.XmlDocument.dll
│   │   ├── System.Xml.XPath.dll
│   │   ├── System.Xml.XPath.XDocument.dll
│   │   ├── vbc.exe
│   │   ├── vbc.exe.config
│   │   ├── vbc.rsp
│   │   ├── VBCSCompiler.exe
│   │   └── VBCSCompiler.exe.config
│   ├── Green Footprint Stateful.dll
│   └── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll
├── obj/Debug/                      ← Build artifacts
│   ├── TempPE/
│   ├── .NETFramework,Version=v4.5.2.AssemblyAttributes.cs
│   ├── DesignTimeResolveAssemblyReferences.cache
│   └── Green Footprint Stateful.dll (multiple build outputs)
├── packages/
│   └── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.2.0.1/
│       ├── build/
│       ├── content/
│       ├── lib/net45/
│       │   ├── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll
│       │   └── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.xml
│       └── tools/
│           ├── net45/
│           ├── Roslyn45/
│           ├── RoslynLatest/
│           ├── .signature.p7s
│           └── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.nupkg
├── Properties/
│   └── AssemblyInfo.cs
├── Cse_445 (VBoxSvr).pubxml        ← Publish profile
├── Global.asax                     ← App/Session event handlers
├── GreenFootprint.asmx             ← Stateful WSDL Web Service
├── GreenFootprint.asmx.cs          ← Service code-behind
├── packages.config
├── ReadMe.md
├── Web.config
├── Web.Debug.config
├── Web.Release.config
├── WebForm1.aspx                   ← Default.aspx (main application page)
├── WebForm1.aspx.cs                ← Code-behind
└── WebForm1.aspx.designer.cs
```

---

## 4. How to Test This Application

### Running Locally
1. Open the `Assignment 5` solution (`.sln`) in Visual Studio
2. Build the solution (`Ctrl+Shift+B`) to restore NuGet packages
3. Press `F5` or click **IIS Express** to launch — `Default.aspx` opens automatically
4. All TryIt sections are visible on `Default.aspx` — no additional navigation required

### Test Cases & Inputs

#### 🌿 Green Footprint Service
| Input | Expected Output |
|-------|----------------|
| Name: `TestUser`, Action: `recycling` | Score +1, state updated |
| Name: `TestUser`, Action: `driving` | Score -1, state updated |
| Get Score for `TestUser` | Displays score + state label |

**State levels:**
| Score Range | State |
|-------------|-------|
| 0 – 25 | 🌱 Sapling |
| 26 – 50 | 🌿 Sprout |
| 51 – 75 | 🪴 Plant |
| 76+ | 🌳 Tree |

#### 🌤 Weather Service
| Input | Expected Output |
|-------|----------------|
| ZIP: `85281` (Tempe, AZ) | 5-day forecast array |
| ZIP: `10001` (New York, NY) | 5-day forecast array |

#### 🌍 Soil Data Service
| Input | Expected Output |
|-------|----------------|
| Query: `loam soil Arizona` | JSON with soil properties |
| Query: `clay composition` | Composition + land suitability |

#### 🔐 Hash Function DLL
| Input | Expected Output |
|-------|----------------|
| Any string in Hash TryIt field | SHA-256 hash displayed in label |

> Verifies local hashing is working without any network transmission.

---

## 5. State Management

| Mechanism | Scope | Stores |
|-----------|-------|--------|
| **Session State** | Per browser session | Username, current score, state level |
| **users.json** | Persistent / permanent | Name, SHA-256 hashed password, accumulated score, state |

Session state is cleared on session end. `users.json` survives application restarts and stores all registered users.

---

## 6. Security

- Passwords are hashed locally using **SHA-256** via DLL before being written to `users.json`
- No plaintext passwords are stored or transmitted over any network
- The hash DLL function runs entirely server-side

---

## 7. Service Directory Table

| Service Name | Type | Provider | Input / Output | Description | TryIt |
|---|---|---|---|---|---|
| `GreenFootprint.asmx` (AddAction, GetScore) | WSDL / SOAP | Person A (Pink) | In: name (string), action (string) / Out: int score, string state | Stateful green footprint tracker. +1 per green action, -1 per non-green. States: Sapling → Sprout → Plant → Tree | Default.aspx → GreenFootprint section |
| Weather Service | WSDL / SOAP | Person B (Purple) | In: U.S. ZIP code (string) / Out: string[] 5-day forecast | Retrieves 5-day weather forecast using NOAA NDFD SOAP service | Default.aspx → Weather section |
| Soil Data Service | RESTful / JSON | Person B (Purple) | In: query string / Out: JSON soil data | Queries USDA NRCS Soil Data Access API for soil composition and land suitability | Default.aspx → Soil section |
| HashPassword DLL | DLL Library Function | Person A (Pink) | In: plaintext string / Out: hashed string | Local SHA-256 hashing for secure credential storage. No network call. | Default.aspx → Hash TryIt button + label |
| Global.asax Handler | Global Event Handler | Person A (Pink) | N/A | `Application_Start` initializes users.json. `Session_Start` logs new sessions. | Implicit — visible via session state in UI |
| Session State | State Management | Person A (Pink) | Stores: username, score, state level | Maintains active user data across page interactions | Default.aspx — score display updates live |
| users.json | Data Layer / JSON | Person A (Pink) | Stores: name, hashedPassword, score, state | Persistent flat-file store in App_Data. Updated on registration and action logging. | Implicit — updated on registration |

---

## 8. Team Contributions

| Team Member | Components Developed | Contribution |
|---|---|---|
| **Person A (Pink)** | Default.aspx UI layout, GreenFootprint.asmx service, HashPassword DLL, Global.asax, Session State, users.json data layer | 50% |
| **Person B (Purple)** | Weather WSDL service integration, Soil Data RESTful service integration, output formatting and error handling | 50% |

---

## 9. Sprint Plan Summary

| Day | Person A (Pink) | Person B (Purple) |
|-----|----------------|-------------------|
| **Monday** | Rename Default.aspx, build UI layout, create Global.asax, write app description | Set up Weather (WSDL) and Soil (REST) services |
| **Tuesday** | Build GreenFootprint.asmx, implement users.json, create hashing DLL | Complete Weather + Soil integrations |
| **Wednesday** | Connect Green service to UI, add Session state, display score levels | Connect Weather + Soil to UI |
| **Thursday** | Validate hashing, JSON updates, UI; add comments | Handle invalid inputs, test API reliability |
| **Friday** | Finalize UI, ensure all TryIt sections visible | Clean service code, remove debug logs |

---

## 10. Server Deployment

- `GreenFootprint.asmx` is deployed to **WebStrar** server for remote testing
- Full application deployment to WebStrar is planned for **Assignment 6**
- Canvas submission includes the complete Visual Studio Solution with both service and application code

---

*CSE 445 | Assignment 5 | Green Footprint Stateful Web Application*