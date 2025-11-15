import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
declare var webkitSpeechRecognition: any;
declare global {
  interface Window {
    webkitSpeechRecognition: any;
    SpeechRecognition: any;
  }
}

@Injectable({
  providedIn: 'root'
})
export class VoiceRecognitionService {

  recognition: any;
  isListening = false;
  transcript = '';

  transcriptSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');
  transcript$: Observable<string> = this.transcriptSubject.asObservable();

  constructor() {
    const SpeechRecognition = window.SpeechRecognition || webkitSpeechRecognition;
    this.recognition = new SpeechRecognition();

    this.recognition.lang = 'en-US';
    this.recognition.interimResults = false;
    this.recognition.maxAlternatives = 1;

    this.recognition.onresult = (event: any) => {
      const result = event.results[0][0].transcript.trim();
      const words = result.split(/\s+/);
    
      if (words.length >= 3) {
        const limitedResult = words.slice(0, 3).join(' ');
        this.transcript = limitedResult;
        this.transcriptSubject.next(limitedResult);
        console.log('Voice input (max 3 words):', limitedResult);
    
        this.recognition.stop(); 
      } else {
        this.transcript = result;
        this.transcriptSubject.next(result);
        console.log('Voice input (partial):', result);
      }
    };
    this.recognition.onerror = (event: any) => {
      console.error('Speech recognition error:', event.error);
    };

    this.recognition.onend = () => {
      this.isListening = false;
    };
  }

  startListening() {
    this.transcript = '';
    this.isListening = true;
    this.recognition.start();
  }

  stopListening() {
    this.recognition.stop();
    this.isListening = false;
  }
}
