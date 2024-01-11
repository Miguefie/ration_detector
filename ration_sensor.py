import RPi.GPIO as GPIO
import time
import requests
import datetime
import json
import logging
import logging.handlers as handlers
import os.path

URL = "http://localhost:5217/ration/silo"

path = './initTime.txt'

initTime = datetime.datetime.now()

if(os.path.isfile(path)):
   f = open(path,"r")
   initTimeFile = f.read()
   initTime = datetime.datetime.strptime(initTimeFile, "%d/%m/%Y, %H:%M:%S")
   f.close()
else:
    f = open(path,"w")
    dateTimeToSave = datetime.datetime.now()
    f.write(dateTimeToSave.strftime("%d/%m/%Y, %H:%M:%S"))
    f.close()


# Custom Formatter
class CustomFormatter(logging.Formatter):
    def format(self, record):
        # Adding custom time attribute
        record.machinetime = str(datetime.datetime.now() - initTime)
        return super(CustomFormatter, self).format(record)

GPIO.setmode(GPIO.BCM)

TRIG = 24
ECHO = 23

GPIO.setup(TRIG,GPIO.OUT)
GPIO.setup(ECHO,GPIO.IN)


logHandler = handlers.TimedRotatingFileHandler('error.log', when='midnight', interval=1, backupCount=30)
logHandler.setLevel(logging.ERROR)

formatter = CustomFormatter('%(machinetime)s - %(name)s - %(levelname)s - %(message)s')
logHandler.setFormatter(formatter)

logger = logging.getLogger('ration_sensor')
logger.addHandler(logHandler)

while True:
    try:
        GPIO.output(TRIG,False)
        time.sleep(5 * 60)

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

        print ("Distance: ",distance,"m")

        machineCurrentTime = datetime.datetime.now() - initTime

        data = {'Distance': distance,
                'CreationDate': ''+str(machineCurrentTime)}

        print (data)
        
        dataJson = json.dumps(data)

        print (dataJson)
        x = requests.post(url=URL, data=dataJson, headers={"Content-Type":"application/json"})

        print(x.text)

    except Exception as e:
        logger.error(e)