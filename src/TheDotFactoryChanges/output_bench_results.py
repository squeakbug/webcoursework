import os
import sys
import json
import socket
import asyncio
import subprocess

from prettytable import PrettyTable

def draw_table(data):
    tab = PrettyTable()
    tab.field_names = ["Название СУБД", "Запрос #1 (выборка без транзакций), з/c", "Запрос #2 (выборка с транзакциями), з/c"]
    for suite in data["suites"]:
        dbname = suite["db-name"]
        data_filename = suite["output"]
        avg1 = avg2 = n = 0
        with open(data_filename, "r") as data_file:
            data_lines = data_file.readlines()
            n = len(data_lines)
            avg1 = sum(float(line.split(',')[0]) for line in data_lines) / n
            avg2 = sum(float(line.split(',')[1]) for line in data_lines) / n
        qps1 = 1000 / avg1
        qps2 = 1000 / avg2
        tab.add_row([dbname, f"{qps1:.3f}", f"{qps2:.3f}"])
    print(tab)

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print(f"Usage: {sys.argv[0]} <config>");
        sys.exit(1)

    filename = sys.argv[1]
    data = None
    with open(filename) as json_file:
        data = json.load(json_file)
    
    draw_table(data)