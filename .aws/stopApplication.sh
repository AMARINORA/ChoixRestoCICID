#!/bin/bash

echo "stop application"
cd  /var/www/choixresto/site/
echo "kill $(ps aux | grep '[C]hoixResto.dll' | awk '{print $2}')"
kill $(ps aux | grep '[C]hoixResto.dll' | awk '{print $2}') || echo "Process ChoixResto.dll was not running."
