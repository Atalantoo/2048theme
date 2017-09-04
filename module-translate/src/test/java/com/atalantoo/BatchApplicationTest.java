package com.atalantoo;

import static java.lang.String.format;
import static org.assertj.core.api.Assertions.assertThat;

import java.util.Arrays;
import java.util.Collection;

import org.junit.Rule;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.Parameterized;
import org.junit.runners.Parameterized.Parameter;
import org.junit.runners.Parameterized.Parameters;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.test.rule.OutputCapture;

@RunWith(Parameterized.class)
public class BatchApplicationTest {

	@Rule
	public OutputCapture outputCapture = new OutputCapture();

	@Parameter
	public String src;
	@Parameter(1)
	public String dest;
	@Parameter(2)
	public String src_lang;
	@Parameter(3)
	public String dest_lang;

	@Parameters( name = "project-2048 {index}: {3}")
	public static Collection<Object[]> data() {
		String path = System.getProperty("user.dir");
		String source = "en";
		String file = path + "\\..\\Project 2048 Cars\\Assets\\Project 2048\\Resources\\i18n\\locale-%s.json";
		String[] targets = new String[] { //
				"ja", //
				"ko", //
				"zh-TW", //
				"de", //
				"fr", //
				"pt", //
				"es", //
				"it", //
				"ru" //
		};
		String[][] parameters = new String[targets.length][4];
		for (int i = 0; i < targets.length; i++) {
			String target = targets[i];
			parameters[i][0] = format(file, source);
			parameters[i][1] = format(file, target);
			parameters[i][2] = source;
			parameters[i][3] = target;
		}
		return Arrays.asList(parameters);
	}

	@Test
	public void generate() throws Exception {
		String ts = String.valueOf(Math.random() * 999999);
		assertThat(SpringApplication.exit(SpringApplication.run(BatchConfig.class,
				new String[] { //
						"--run.id=" + ts, //
						"--src=" + src, //
						"--dest=" + dest, //
						"--src_lang=" + src_lang, //
						"--dest_lang=" + dest_lang }))).isEqualTo(0);
		String output = this.outputCapture.toString();
		assertThat(output).contains("completed with the following parameters");
	}

}