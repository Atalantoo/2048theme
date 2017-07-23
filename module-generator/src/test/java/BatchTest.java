import static java.lang.String.format;

import org.junit.Test;

public class BatchTest {

	static String path = System.getProperty("user.dir");
	static String src = path + "\\..\\resources\\%s.csv";
	static String dest_sandalone = path + "\\..\\app-%s\\Assets\\%s\\Sprites";
	static String dest_mainapp = path + "\\..\\app\\Assets\\%s\\Sprites";

	String[] args = new String[] { //
			"cars"//
	};

	@Test
	public void all_apps() {
		for (String arg : args) {
			Batch.main(new String[] { //
					"--src=" + format(src, arg), //
					"--dest=" + format(dest_sandalone, arg, arg) });
		}
	}

	@Test
	public void main_app() {
		for (String arg : args) {
			Batch.main(new String[] { //
					"--src=" + format(src, arg), //
					"--dest=" + format(dest_mainapp, arg) });
		}
	}

	@Test
	public void one_job() {
		String arg = "cars";
		Batch.main(new String[] { //
				"--src=" + format(src, arg), //
				"--dest=" + format(dest_sandalone, arg, arg) });
	}
}
