from pythonosc.dispatcher import Dispatcher
from pythonosc import osc_server, udp_client

# Based on python-osc examples - https://github.com/attwad/python-osc

# Set up both client and dispatcher to send and recieve messages
# Do note that Threading Server blocks further execution therefore
# last print does not occur, will have to switch to other server type 
# if your final project requires further execution within Python
# Docs : https://python-osc.readthedocs.io/en/latest/server.html

IP = "127.0.0.1"  
RECEIVE_PORT = 5005      
SEND_PORT = 6969          

client = udp_client.SimpleUDPClient(IP, SEND_PORT)

def response(unused_addr, arg):
    multiplied_number = arg * 5
    print(f"Sending {multiplied_number} on port {SEND_PORT}")
    client.send_message("/ReceivedMessage",multiplied_number)

disp = Dispatcher()
disp.map("/SentMessage", response)

if __name__ == "__main__":
    server = osc_server.ThreadingOSCUDPServer((IP, RECEIVE_PORT), disp)
    print("Server is up")
    print(f"IP : {IP}")
    print(f"Receiving Port : {RECEIVE_PORT}")
    server.serve_forever()