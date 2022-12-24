import os
import sys
import json
import socket
import asyncio
import subprocess

from prettytable import PrettyTable

def get_data_from(host, port):
    backlog = 5

    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server_address = (host, port)
    sock.bind(server_address)
    sock.listen(backlog)

    print ("Waiting to receive message from client")
    client, address = sock.accept()
    data = client.recv(1024)
    str_data = ""
    if data:
        str_data = data.decode("utf-8")
    client.close()
    sock.close()
    return str_data

def draw_table(data):
    tab = PrettyTable()
    tab.field_names = ["Название СУБД", "Запрос #1 \n (выборка без транзакций), з/c", "Запрос #2 \n (выборка с транзакциями), з/c"]
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

def main(data):
    host_ip = data["host-ip"]
    port_number = int(data["port-number"])
    runs_count = int(data["runs-count"])
    for suite in data["suites"]:
        docker_compose_args = suite["docker-compose-args"]
        docker_build(suite)
        for i in range(runs_count):
            docker_run(suite, docker_compose_args, host_ip, port_number)
            docker_down(suite)
    draw_table(data)

def docker_build(suite):
    exec_args = ["docker-compose", "-f", suite['file'], "build"]
    print("exec", " ".join(exec_args))
    p = subprocess.Popen(exec_args)
    p.wait()

def docker_run(suite, args, host_ip, port_number):
    exec_args = ["docker-compose", "-f", suite['file'], "run"] + args
    print("exec", " ".join(exec_args))
    p = subprocess.Popen(exec_args)
    data = get_data_from(host_ip, port_number)
    with open(suite["output"], "a") as data_file:
        data_file.write(data.strip('\n\r'))
        data_file.write("\n")
    p.wait()

def docker_down(suite):
    exec_args = ["docker-compose", "-f", suite['file'], "down"]
    print("exec", " ".join(exec_args))
    p = subprocess.Popen(exec_args)
    p.wait()

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print(f"Usage: {sys.argv[0]} <config>");
        sys.exit(1)

    filename = sys.argv[1]
    data = None
    with open(filename) as json_file:
        data = json.load(json_file)

    main(data)
    sys.exit(0)
