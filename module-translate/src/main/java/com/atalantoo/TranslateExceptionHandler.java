package com.atalantoo;
import org.springframework.batch.repeat.RepeatContext;
import org.springframework.batch.repeat.exception.ExceptionHandler;

public class TranslateExceptionHandler implements ExceptionHandler {

	@Override
	public void handleException(RepeatContext context, Throwable throwable) throws Throwable {
		// TODO Auto-generated method stub
		System.out.println("hello");
	}

}
