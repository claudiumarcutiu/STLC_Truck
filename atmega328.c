#ifndef F_CPU 
#define F_CPU 16000000UL 
#endif

#include <AVR/IO.h>
#include <AVR/interrupt.h>
#include <math.h>
#include <util/delay.h>
#include <string.h>
#include <stdio.h> 
#include <stdlib.h>


#define BAUD 9600
#define ubrr 51
const double pi = 3.14159265358979323846;
int i=1,contor_led,u=0,cont_tmr1=0,cont_tmr0=0,t=0,t1=0;
volatile int j=1;
long suma=0;
int timpSenzorUS=0;
int timpPauzaSenzorUS=0;
//float y,z;
unsigned char r,c;
char buff[100];

void initserial(void){
SREG=SREG&127;
UCSR0B=(1<<RXEN0)|(1<<TXEN0);//enable transmiter and reciever
UBRR0H=(unsigned char) (ubrr>>8);//set transfer rate
UBRR0L=(unsigned char) ubrr ;
UCSR0C=0x86;//8 data bit+1 stop data
UCSR0B=UCSR0B|192;//enable USART interupt TXCIE/RXCIE

//UCSRA=UCSRA|128;//activare recieve complete interupt
SREG=SREG|128;
}


//****************** INTRERUPERE TIMER0************
ISR(TIMER0_OVF_vect)
{


 


//TIMSK=TIMSK&254;
}//************************************************


int prints(char *string) 
{ 
    
   int count =0; 
   while ((string[count]) != '\0') 
   { 
      while ( !( UCSR0A & (1<<UDRE0)) );  // Wait for empty transmit buffer 
      UDR0 = (char)string[count++]; 
          
   }    
   //TxByte('_');
    UCSR0A=UCSR0A & 32;
   return 0; 

}



ISR (USART_TX_vect){

}

//****************** INTRERUPERE RECIEVE COMPLETE************
ISR (USART_RX_vect){
//ISR(USART_RXC_vect ){

unsigned char r;



 r=UDR0;

 sprintf(buff,"  %d \n\r ",r); 
				

if (r>=0&&r<=29) 
 { 
PORTB=PORTB|32;
PORTB=PORTB&239;
sprintf(buff,"  3 \n\r "); 
 }
else if (r>=30&&r<=59)
 {
PORTB=PORTB|16;
PORTB=PORTB&223;
sprintf(buff,"  4 \n\r "); 
 } 
else if(r>=60&&r<=89)
 {
PORTB=PORTB|1;
PORTD=PORTD&127;
sprintf(buff,"  5 \n\r "); 
 }
else if(r>=90&&r<=119)
 {
PORTD=PORTD|128;
PORTB=PORTB&254;
sprintf(buff,"  6 \n\r "); 
 }
else if(r>=120&&r<=149)
 {
PORTD=PORTD|64;
PORTD=PORTD&239;
sprintf(buff,"  7 \n\r "); 
 }
 else if(r>=150&&r<=179)
 {
PORTD=PORTD|16;
PORTD=PORTD&191;
sprintf(buff,"  8 \n\r "); 
 }
 else if(r>=180&&r<=209)
 {
PORTD=PORTD|4;
PORTD=PORTD&247;
sprintf(buff,"  9 \n\r "); 
 }
  else if(r>=210&&r<=239)
 {
PORTD=PORTD|8;
PORTD=PORTD&251;
sprintf(buff,"  10 \n\r "); 
 }


 prints(buff);
}



int main(void)
{
 //DDRD=DDRD|224; // pin 7,6,5 config ca iesiri
 //DDRB=DDRB|6; //pin pb1, pb2 ca iesiri
 
 
 DDRB = DDRB| 49;  //inputurile 1,2,3 de la primul l298
 DDRD = DDRD| 128; //input 4 primul l298


 DDRD = DDRD|92; //toate inputurile de la al doilea l298
//LM 1
 DDRB = DDRB| 8;
 DDRB = DDRB| 4;

//PWM-URI
 PORTB = PORTB| 8;
 PORTB = PORTB| 4;
 PORTB = PORTB | 2;
 PORTD = PORTD | 32;	
//ENABLE-URI
 PORTB = PORTB| 1;
 PORTD = PORTD& 127;

 PORTB = PORTB| 32;
 PORTB = PORTB& 239;

 PORTD = PORTD | 128;
 PORTB = PORTB & 254;

 PORTD = PORTD | 64;
 PORTD = PORTD & 239;

 PORTD = PORTD | 4;
 PORTD = PORTD & 247;


initserial();	
	PORTD=PORTD|128;

	sprintf(buff,"  Mama Lena da-mi bani! \n\r "); 
				prints(buff);

SREG=SREG|128;
	while(1)
	{
	
	
			
		/*PORTD=PORTD|128;
		PORTD=PORTD&223;//59
		PORTD=PORTD&191;
		_delay_ms(1000);
		PORTD=PORTD|64;
		PORTD=PORTD&223;
		PORTD=PORTD&127;
		_delay_ms(1000);
		PORTD=PORTD|32;
		PORTD=PORTD&127;
		PORTD=PORTD&191;
		_delay_ms(1000);*/
	}

return 0;
}
