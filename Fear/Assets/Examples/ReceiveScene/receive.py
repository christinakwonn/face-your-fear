import random
import time
from pythonosc import udp_client

# Based on python-osc examples - https://github.com/attwad/python-osc

# Sets up our client with IP and Port then sends a random number (0-1) 
# 10 times each second, ReceivedMessage is the address of where the message 
# is being sent and can be changed according to what your receiver address is


IP = "127.0.0.1"
SEND_PORT = "6969"

if __name__ == "__main__":
    client = udp_client.SimpleUDPClient(IP, SEND_PORT)
    for x in range(10):
        num = random.random()
        print(num)
        client.send_message("/ReceivedMessage", num)
        time.sleep(1)
