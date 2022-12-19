import os
import sys
import json

from prettytable import PrettyTable

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print(f"Usage: {sys.argv[0]} <config>");
        sys.exit(1)
    
    filename = sys.argv[1]
    data = None
    with open(filename) as json_file:
        data = json.load(json_file)

    for suite in data["suites"]:
        docker_compose_args = suite["docker-compose-args"]
        args_string = " ".join(docker_compose_args)
        build_cmd = f"docker-compose -f {suite['file']} build"
        print("exec", build_cmd)
        os.system(build_cmd)
        run_cmd = f"docker-compose -f {suite['file']} run {args_string}"
        print("exec", run_cmd)
        os.system(run_cmd)
        down_cmd = f"docker-compose -f {suite['file']} down"
        print("exec", down_cmd)
        os.system(down_cmd)
        sys.exit(0)
