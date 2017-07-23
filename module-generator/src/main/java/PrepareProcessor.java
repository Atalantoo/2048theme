import static com.google.common.base.Preconditions.checkArgument;
import static java.lang.String.format;
import static org.apache.commons.lang3.StringUtils.isNotBlank;

import java.util.ArrayList;

import org.apache.commons.io.FilenameUtils;
import org.springframework.batch.item.ItemProcessor;
import org.springframework.core.io.Resource;

import lombok.NonNull;
import lombok.RequiredArgsConstructor;

@RequiredArgsConstructor
public class PrepareProcessor implements ItemProcessor<Prepare, Prepare> {

	@NonNull
	private Resource src;
	@NonNull
	private Resource des;

	private final static String[] FILES = new String[] { //
			"0000", //
			"0002", //
			"0004", //
			"0008", //
			"0016", //
			"0032", //
			"0064", //
			"0128", //
			"0256", //
			"0512", //
			"1024", //
			"2048" };

	@Override
	public Prepare process(Prepare item) throws Exception {
		checkArgument(isNotBlank(item.getName()));
		String app = FilenameUtils.removeExtension(src.getFilename());
		item.setPictures(new ArrayList<>());
		for (String sprite : FILES) {
			String srcStr = format("%s/%s/%s/%s.png", src.getFile().getParentFile().toPath().toString(), //
					app, item.getName(), sprite);
			String desStr = format("%s/%s/%s.png", des.getFile().toPath().toString(), //
					item.getName(), sprite);
			item.getPictures() //
					.add( //
							new Picture(srcStr, desStr));
		}
		return item;
	}

}
