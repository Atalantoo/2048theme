package com.atalantoo;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;

import org.apache.commons.lang3.StringUtils;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.htmlunit.HtmlUnitDriver;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.springframework.batch.item.ItemProcessor;

import com.google.common.base.Joiner;
import com.google.common.base.Preconditions;

import lombok.Data;
import lombok.NonNull;
import lombok.RequiredArgsConstructor;

@Data
@RequiredArgsConstructor
public class TranslateProcessor implements ItemProcessor<LocaleJSONLine, LocaleJSONLine> {

	@NonNull
	public String src_lang;
	@NonNull
	public String dest_lang;

	private static final String API_PATTERN = "https://translate.google.fr/#%s/%s/%s";
	private static final String API_TARGET = "#result_box span";
	private static final String LINE_BREAK = "\\n";
	private static final String LINE_BREAK_REGEX = "\\\\n";
	private static Joiner joiner = Joiner.on(LINE_BREAK).skipNulls();

	@Override
	public LocaleJSONLine process(LocaleJSONLine item) throws Exception {
		if (item.key == null)
			return null;
		Preconditions.checkArgument(StringUtils.isNotBlank(item.value));

		String newValue;
		if (item.value.contains(LINE_BREAK)) {
			String[] lines = item.value.split(LINE_BREAK_REGEX);
			for (int i = 0; i < lines.length; i++) {
				lines[i] = translate(lines[i]);
			}
			newValue = joiner.join(lines);
		} else {
			newValue = translate(item.value);
		}

		return new LocaleJSONLine(item.key, newValue);
	}

	private String translate(String value) throws UnsupportedEncodingException {
		String urlValue = URLEncoder.encode(value, "UTF-8");
		String url = String.format(API_PATTERN, src_lang, dest_lang, urlValue);

		System.setProperty("jsse.enableSNIExtension", "false");
		DesiredCapabilities cap = new DesiredCapabilities();
		WebDriver webDriver = new HtmlUnitDriver(cap);
		webDriver.get(url);

		WebDriverWait wait = new WebDriverWait(webDriver, 5);
		wait.until(ExpectedConditions.elementToBeClickable(By.cssSelector(API_TARGET)));
		WebElement target = webDriver.findElement(By.cssSelector(API_TARGET));
		String newValue = target.getText();

		webDriver.close();
		return newValue;
	}

	private String translate2(String value) throws UnsupportedEncodingException {

		return null;
	}
}
