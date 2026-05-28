import std/[os, strutils, sequtils, json, times, cpuinfo, parseopt]

type
  SystemInfo = object
    hostname: string
    os: string
    cpu: string
    cpuCores: int
    memoryTotal: int64
    processes: seq[ProcessInfo]

  ProcessInfo = object
    pid: int
    name: string
    state: string

proc getSystemInfo(): SystemInfo =
  result.hostname = try: hostname() except: "unknown"
  result.os = hostOS
  result.cpu = hostCPU
  result.cpuCores = try: countProcessors() else: 0
  result.memoryTotal = try: getTotalMem() else: 0
  result.processes = @[]

proc parseLinuxMemInfo(): Table[string, int64] =
  result = initTable[string, int64]()
  try:
    for line in lines("/proc/meminfo"):
      let parts = line.splitWhitespace()
      if parts.len >= 2:
        let key = parts[0].strip(':')
        let value = parts[1].parseInt()
        result[key] = value
  except:
    discard

proc getProcList(): seq[ProcessInfo] =
  result = @[]
  try:
    for kind, path in walkDir("/proc"):
      if kind != pcDir: continue
      let dirName = path.splitPath().tail
      let pid = try: dirName.parseInt() except: continue

      var name = "unknown"
      var state = "?"
      try:
        let lines = readFile(path / "status").splitLines()
        for line in lines:
          if line.startsWith("Name:"):
            name = line.splitWhitespace()[1]
          if line.startsWith("State:"):
            state = line.splitWhitespace()[1]
      except:
        continue

      result.add(ProcessInfo(pid: pid, name: name, state: state))
  except:
    discard

proc printSystemInfo(info: SystemInfo) =
  echo "=== System Information ==="
  echo "Hostname: ", info.hostname
  echo "OS: ", info.os
  echo "CPU: ", info.cpu
  echo "Cores: ", info.cpuCores

  when defined(linux):
    let memInfo = parseLinuxMemInfo()
    if memInfo.hasKey("MemTotal"):
      echo "Memory Total: ", memInfo["MemTotal"] div 1024, " MB"
    if memInfo.hasKey("MemFree"):
      echo "Memory Free: ", memInfo["MemFree"] div 1024, " MB"
    if memInfo.hasKey("MemAvailable"):
      echo "Memory Available: ", memInfo["MemAvailable"] div 1024, " MB"

  echo "\n=== Processes ({info.processes.len}) ==="
  echo fmt"{'PID':>8} {'NAME':<20} {'STATE':<8}"
  echo repeat('-', 40)
  for p in info.processes[0 .. min(info.processes.len - 1, 20)]:
    echo fmt"{p.pid:>8} {p.name:<20} {p.state:<8}"
  if info.processes.len > 21:
    echo "... and ", info.processes.len - 21, " more"

proc toJson(info: SystemInfo): JsonNode =
  var procArr = newJArray()
  for p in info.processes:
    procArr.add(%*{
      "pid": p.pid,
      "name": p.name,
      "state": p.state
    })

  result = %*{
    "hostname": info.hostname,
    "os": info.os,
    "cpu": info.cpu,
    "cpuCores": info.cpuCores,
    "processCount": info.processes.len,
    "processes": procArr
  }

proc main() =
  var outputJson = false
  var section = "all"

  var args = commandLineParams()
  var i = 0
  while i < args.len:
    case args[i]
    of "--json":
      outputJson = true
    of "cpu", "memory", "processes":
      section = args[i]
    else:
      discard
    i.inc

  var info = getSystemInfo()

  case section
  of "processes":
    info.processes = getProcList()
    if outputJson:
      echo info.toJson()["processes"].pretty
    else:
      echo "=== Processes ==="
      for p in info.processes:
        echo fmt"{p.pid:>8} {p.name:<20} {p.state:<8}"
  of "cpu":
    if outputJson:
      echo %*{"cpu": info.cpu, "cores": info.cpuCores}
    else:
      echo "CPU: ", info.cpu
      echo "Cores: ", info.cpuCores
  of "memory":
    when defined(linux):
      let memInfo = parseLinuxMemInfo()
      if outputJson:
        echo %memInfo
      else:
        for k, v in memInfo:
          echo k, ": ", v div 1024, " MB"
    else:
      echo "Memory info available on Linux only"
  else:
    info.processes = getProcList()
    if outputJson:
      echo info.toJson().pretty
    else:
      printSystemInfo(info)

when isMainModule:
  main()
