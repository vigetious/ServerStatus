case "$1" in 
cpuCount)
  grep ^cpu\\scores /proc/cpuinfo | uniq |  awk '{print $4}'
  ;;
cpuTemp)
  sensors -j
  ;;
esac