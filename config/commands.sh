case "$1" in 
cpuPhysicalCount)
  grep ^cpu\\scores /proc/cpuinfo | uniq |  awk '{print $4}'
  ;;
cpuCount)
  nproc
  ;;
cpuTemp)
  sensors -j
  ;;
memoryUsed)
  free -m | awk 'FNR == 2 {print $3}'
  ;;
memoryTotal)
  free -m | awk 'FNR == 2 {print $2}'
  ;;
networkInterfaces)
  ip -j link
  ;;
networkSpeed)
  cat /sys/class/net/"$2"/statistics/"$3"
  ;;
esac