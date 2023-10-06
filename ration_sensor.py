import RPi.GPIO as GPIO
import time
import requests
import datetime
import json


URL = "http://localhost:5217/ration/silo"

GPIO.setmode(GPIO.BCM)

TRIG = 24
ECHO = 23

GPIO.setup(TRIG,GPIO.OUT)
GPIO.setup(ECHO,GPIO.IN)

try:
    while True:
        GPIO.output(TRIG,False)
        time.sleep(2)

        GPIO.output(TRIG,True)
        time.sleep(0.00001)
        GPIO.output(TRIG,False)


        while GPIO.input(ECHO)==0:
            pulse_start = time.time()
            
        while GPIO.input(ECHO)==1:
            pulse_end = time.time()
            
        pulse_duration = pulse_end - pulse_start

        distance = pulse_duration * 171.50

        distance = round(distance,2)

        print "Distance: ",distance,"m"


        now = datetime.datetime.now()

        data = {'Distance': distance,
                'CreationDate': ''+now.strftime("%m/%d/%Y, %H:%M:%S")}

        print data
        
        dataJson = json.dumps(data)

        print dataJson

        x = requests.post(url=URL, data=dataJson, headers={"Content-Type":"application/json"})

        print(x.text)

        
        
except KeyboardInterrupt:
    pass